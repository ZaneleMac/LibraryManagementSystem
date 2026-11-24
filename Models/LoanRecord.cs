using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementSystem.Models;

public class LoanRecord
{
    public string BookId { get; }
    public string MemberId { get; }
    public DateTime BorrowedOn { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedOn { get; private set; }

    public LoanRecord(string bookId, string memberId, DateTime borrowedOn, DateTime dueDate)
    {
        BookId = bookId;
        MemberId = memberId;
        BorrowedOn = borrowedOn;
        DueDate = dueDate;
    }

    public bool IsReturned => ReturnedOn.HasValue;

    public bool IsOverdue(DateTime asOfDate) =>
        !IsReturned && asOfDate.Date > DueDate.Date;

    public void MarkReturned(DateTime returnedDate)
    {
        if (IsReturned)
            throw new InvalidOperationException("Loan has already been returned.");

        ReturnedOn = returnedDate;
    }
}