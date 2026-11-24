using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementSystem.Models
{
    
    public record LoanReceipt(
        string Action,
        string BookTitle,
        string MemberName,
        string MembershipType,
        DateTime TransactionDate,
        DateTime DueDate,
        bool WasOverdue
    );
}
