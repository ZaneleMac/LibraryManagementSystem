# Library Management System

A console-based **Library Management System** built in C# and .NET demonstrating object-oriented programming (OOP) principles, domain validation, unit testing with xUnit, and immutable C# `record` types.

## Features

- **Book Management:** Add books with validated IDs, titles, authors, and copy counts while tracking available vs. total copies.
- **Member Tiers & Borrowing Rules:** Supports multiple member types with distinct loan periods and active loan limits via inheritance and polymorphism.
- **Borrowing & Returns:** Handles book checkouts and returns, calculates due dates automatically, flags overdue returns, and prevents invalid states (out-of-stock books, exceeding loan caps, duplicate active loans, or returning unborrowed books).
- **Overdue Tracking:** Identifies active loans past their due date.
- **Transaction Receipts:** Uses a C# `record` (`LoanReceipt`) to return an immutable summary of borrow and return transactions.

## OOP & C# Design

- **Encapsulation:** `Book` and `LoanRecord` protect their state with private setters and validated methods (`BorrowCopy`, `ReturnCopy`, `MarkReturned`), while `LibraryService` coordinates lookups and business rules.
- **Inheritance & Polymorphism:** `Member` is an abstract base class defining `LoanPeriodDays`, `MaxActiveLoans`, and `CalculateDueDate(...)`, overridden by `RegularMember` and `StudentMember`.
- **C# `record` Types:** `LoanReceipt` models read-only transaction results with value-based equality and clean formatting.

#### 1. Prerequisites
Ensure the following tools are installed on your machine:
- [.NET SDK](https://dotnet.microsoft.com/download) (**.NET 8.0** or later)
- [Git](https://git-scm.com/)
- [Visual Studio 2022/2026](https://visualstudio.microsoft.com/) (with the **.NET desktop development** workload) or any code editor with terminal access

#### 2. Clone the Repository
Open a terminal or command prompt and run:
```bash
git clone [https://github.com/ZaneleMac/](https://github.com/ZaneleMac/LibaryManagementSystem.git)
cd LibraryManagementSystem

## Borrowing Rules

| Member Type | Loan Period | Max Active Loans |
| :--- | :--- | :--- |
| **Regular** (`RegularMember`) | 14 days | 3 |
| **Student** (`StudentMember`) | 21 days | 5 |

## Project Structure

```text
LibraryManagementSystem/
├── Models/
│   ├── Book.cs
│   ├── Member.cs
│   ├── LoanRecord.cs
│   └── LoanReceipt.cs
├── Services/
│   └── LibraryService.cs
├── Program.cs
└── LibraryManagementSystem.csproj

LibraryManagementSystem.Tests/
└── LibraryServiceTests.cs
