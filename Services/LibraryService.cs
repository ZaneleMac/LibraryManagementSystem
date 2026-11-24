using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

public class LibraryService
{
    private readonly Dictionary<string, Book> _books = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Member> _members = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<LoanRecord> _loans = [];

    public IReadOnlyCollection<Book> Books => _books.Values;
    public IReadOnlyCollection<Member> Members => _members.Values;
    public IReadOnlyList<LoanRecord> Loans => _loans;

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (!_books.TryAdd(book.Id, book))
            throw new InvalidOperationException($"Book ID '{book.Id}' already exists.");
    }

    public void RegisterMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);
        if (!_members.TryAdd(member.Id, member))
            throw new InvalidOperationException($"Member ID '{member.Id}' already exists.");
    }

    public LoanReceipt BorrowBook(string memberId, string bookId, DateTime? borrowDate = null)
    {
        var member = GetMemberOrThrow(memberId);
        var book = GetBookOrThrow(bookId);
        var date = (borrowDate ?? DateTime.Today).Date;

        int activeLoans = _loans.Count(l =>
            l.MemberId.Equals(member.Id, StringComparison.OrdinalIgnoreCase) && !l.IsReturned);

        if (activeLoans >= member.MaxActiveLoans)
            throw new InvalidOperationException(
                $"{member.FullName} ({member.MembershipType}) reached the max limit of {member.MaxActiveLoans} active loans.");

        bool duplicateActiveLoan = _loans.Any(l =>
            l.MemberId.Equals(member.Id, StringComparison.OrdinalIgnoreCase) &&
            l.BookId.Equals(book.Id, StringComparison.OrdinalIgnoreCase) &&
            !l.IsReturned);

        if (duplicateActiveLoan)
            throw new InvalidOperationException($"{member.FullName} already borrowed '{book.Title}'.");

        book.BorrowCopy();
        var dueDate = member.CalculateDueDate(date);
        _loans.Add(new LoanRecord(book.Id, member.Id, date, dueDate));

        return new LoanReceipt("Borrowed", book.Title, member.FullName, member.MembershipType, date, dueDate, false);
    }

    public LoanReceipt ReturnBook(string memberId, string bookId, DateTime? returnDate = null)
    {
        var member = GetMemberOrThrow(memberId);
        var book = GetBookOrThrow(bookId);
        var date = (returnDate ?? DateTime.Today).Date;

        var activeLoan = _loans.FirstOrDefault(l =>
            l.MemberId.Equals(member.Id, StringComparison.OrdinalIgnoreCase) &&
            l.BookId.Equals(book.Id, StringComparison.OrdinalIgnoreCase) &&
            !l.IsReturned);

        if (activeLoan is null)
            throw new InvalidOperationException($"No active loan found for '{book.Title}' by {member.FullName}.");

        if (date < activeLoan.BorrowedOn.Date)
            throw new ArgumentException("Return date cannot be before borrow date.", nameof(returnDate));

        bool wasOverdue = activeLoan.IsOverdue(date);
        activeLoan.MarkReturned(date);
        book.ReturnCopy();

        return new LoanReceipt("Returned", book.Title, member.FullName, member.MembershipType, date, activeLoan.DueDate, wasOverdue);
    }

    public IEnumerable<LoanRecord> GetOverdueLoans(DateTime? asOfDate = null)
    {
        var checkDate = (asOfDate ?? DateTime.Today).Date;
        return _loans.Where(l => l.IsOverdue(checkDate));
    }

    private Book GetBookOrThrow(string bookId)
    {
        if (string.IsNullOrWhiteSpace(bookId) || !_books.TryGetValue(bookId.Trim(), out var book))
            throw new KeyNotFoundException($"Book '{bookId}' not found.");
        return book;
    }

    private Member GetMemberOrThrow(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId) || !_members.TryGetValue(memberId.Trim(), out var member))
            throw new KeyNotFoundException($"Member '{memberId}' not found.");
        return member;
    }
}
