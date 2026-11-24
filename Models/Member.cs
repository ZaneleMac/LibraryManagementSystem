using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementSystem.Models
{
    public abstract class Member
    {
        public string Id { get; }
        public string FullName { get; private set; }

        protected Member(string id, string fullName)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Member ID is required.", nameof(id));
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Member name is required.", nameof(fullName));

            Id = id.Trim();
            FullName = fullName.Trim();
        }

        public abstract string MembershipType { get; }
        public abstract int LoanPeriodDays { get; }
        public abstract int MaxActiveLoans { get; }

        public DateTime CalculateDueDate(DateTime borrowDate) =>
            borrowDate.Date.AddDays(LoanPeriodDays);
    }

    public sealed class RegularMember : Member
    {
        public RegularMember(string id, string fullName) : base(id, fullName) { }

        public override string MembershipType => "Regular";
        public override int LoanPeriodDays => 14;
        public override int MaxActiveLoans => 3;
    }

    public sealed class StudentMember : Member
    {
        public StudentMember(string id, string fullName) : base(id, fullName) { }

        public override string MembershipType => "Student";
        public override int LoanPeriodDays => 21;
        public override int MaxActiveLoans => 5;
    }

}
