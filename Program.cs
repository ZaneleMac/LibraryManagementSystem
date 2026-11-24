using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

var library = new LibraryService();
SeedSampleData(library);

while (true)
{
    Console.WriteLine("\n=== Library Management System ===");
    Console.WriteLine("1. List Books");
    Console.WriteLine("2. List Members & Borrowing Rules");
    Console.WriteLine("3. Add Book");
    Console.WriteLine("4. Register Member");
    Console.WriteLine("5. Borrow Book");
    Console.WriteLine("6. Return Book");
    Console.WriteLine("7. View Overdue Loans");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    var choice = Console.ReadLine()?.Trim();

    try
    {
        switch (choice)
        {
            case "1":
                ListBooks(library);
                break;
            case "2":
                ListMembers(library);
                break;
            case "3":
                AddBookFlow(library);
                break;
            case "4":
                RegisterMemberFlow(library);
                break;
            case "5":
                BorrowBookFlow(library);
                break;
            case "6":
                ReturnBookFlow(library);
                break;
            case "7":
                ListOverdueLoans(library);
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid option. Please choose a number from the menu.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void SeedSampleData(LibraryService library)
{
    library.AddBook(new Book("B1", "Clean C", "Robert C. Martin", 2));
    library.AddBook(new Book("B2", "C# in Depth", "Jon Skeet", 1));
    library.AddBook(new Book("B3", "Designing Data-Intensive Applications", "Martin Kleppmann", 2));

    library.RegisterMember(new RegularMember("M1", "Thabo Molefe"));
    library.RegisterMember(new StudentMember("M2", "Lerato Dlamini"));
}

static void ListBooks(LibraryService library)
{
    Console.WriteLine("\n--- Books ---");
    foreach (var b in library.Books)
    {
        Console.WriteLine($"[{b.Id}] {b.Title} by {b.Author} | Available: {b.AvailableCopies}/{b.TotalCopies}");
    }
}

static void ListMembers(LibraryService library)
{
    Console.WriteLine("\n--- Members & Rules ---");
    foreach (var m in library.Members)
    {
        Console.WriteLine($"[{m.Id}] {m.FullName} ({m.MembershipType}) | Loan Period: {m.LoanPeriodDays} days | Max Active Loans: {m.MaxActiveLoans}");
    }
}

static void AddBookFlow(LibraryService library)
{
    Console.Write("Book ID: ");
    var id = Console.ReadLine() ?? "";
    Console.Write("Title: ");
    var title = Console.ReadLine() ?? "";
    Console.Write("Author: ");
    var author = Console.ReadLine() ?? "";
    Console.Write("Total Copies: ");

    if (!int.TryParse(Console.ReadLine(), out int copies))
    {
        Console.WriteLine("Please enter a valid whole number for copies.");
        return;
    }

    library.AddBook(new Book(id, title, author, copies));
    Console.WriteLine("Book added successfully.");
}

static void RegisterMemberFlow(LibraryService library)
{
    Console.Write("Member ID: ");
    var id = Console.ReadLine() ?? "";
    Console.Write("Full Name: ");
    var name = Console.ReadLine() ?? "";
    Console.Write("Membership Type (1 = Regular [14 days, max 3], 2 = Student [21 days, max 5]): ");
    var type = Console.ReadLine()?.Trim();

    Member member = type switch
    {
        "1" => new RegularMember(id, name),
        "2" => new StudentMember(id, name),
        _ => throw new ArgumentException("Invalid membership type. Choose 1 for Regular or 2 for Student.")
    };

    library.RegisterMember(member);
    Console.WriteLine($"{member.MembershipType} member '{member.FullName}' registered.");
}

static void BorrowBookFlow(LibraryService library)
{
    Console.Write("Member ID: ");
    var memberId = Console.ReadLine() ?? "";
    Console.Write("Book ID: ");
    var bookId = Console.ReadLine() ?? "";

    var receipt = library.BorrowBook(memberId, bookId);
    PrintReceipt(receipt);
}

static void ReturnBookFlow(LibraryService library)
{
    Console.Write("Member ID: ");
    var memberId = Console.ReadLine() ?? "";
    Console.Write("Book ID: ");
    var bookId = Console.ReadLine() ?? "";

    var receipt = library.ReturnBook(memberId, bookId);
    PrintReceipt(receipt);
}

static void ListOverdueLoans(LibraryService library)
{
    var overdue = library.GetOverdueLoans().ToList();
    Console.WriteLine("\n--- Overdue Loans ---");
    if (overdue.Count == 0)
    {
        Console.WriteLine("No overdue loans as of today.");
        return;
    }

    foreach (var loan in overdue)
    {
        Console.WriteLine($"Book ID: {loan.BookId} | Member ID: {loan.MemberId} | Due: {loan.DueDate:yyyy-MM-dd}");
    }
}

static void PrintReceipt(LoanReceipt receipt)
{
    Console.WriteLine($"\n[{receipt.Action}] {receipt.BookTitle} -> {receipt.MemberName} ({receipt.MembershipType})");
    Console.WriteLine($"Date: {receipt.TransactionDate:yyyy-MM-dd} | Due Date: {receipt.DueDate:yyyy-MM-dd} | Overdue: {(receipt.WasOverdue ? "Yes" : "No")}");
}