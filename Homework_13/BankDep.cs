using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Homework_13
{
    public enum BankDepartment
    {
        IndividualBank,
        BusinessBank,
        VipBank
    }

    public abstract class BankDep
    {
        public ObservableCollection<Client> Clients { get; set; }
        public abstract string Department { get; set; }
    }

    internal class IndividBank : BankDep
    {
        public override string Department { get; set; } = "Individual";

        public IndividBank()
        {
            Clients = new ObservableCollection<Client>();
        }
    }

    internal class BusinessBank : BankDep
    {
        public override string Department { get; set; } = "Business";

        public BusinessBank()
        {
            Clients = new ObservableCollection<Client>();
        }
    }

    internal class VipBank : BankDep
    {
        public override string Department { get; set; } = "VIP";

        public VipBank()
        {
            Clients = new ObservableCollection<Client>();
        }
    }
}
