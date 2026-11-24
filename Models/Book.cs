using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace LibraryManagementSystem.Models;

public class Book
{
    public string Id { get; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    public Book(string id, string title, string author, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Book ID is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Book author is required.", nameof(author));
        if (totalCopies <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalCopies), "Total copies must be greater than zero.");

        Id = id.Trim();
        Title = title.Trim();
        Author = author.Trim();
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public bool IsAvailable => AvailableCopies > 0;

    public void BorrowCopy()
    {
        if (!IsAvailable)
            throw new InvalidOperationException($"No copies of '{Title}' are available.");

        AvailableCopies--;
    }

    public void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new InvalidOperationException($"All copies of '{Title}' are already in stock.");

        AvailableCopies++;
    }
}
