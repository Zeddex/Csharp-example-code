﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Homework_13
{

    #region HW13

    // Создать прототип банковской системы, позволяющей управлять клиентами и клиентскими счетами.
    // В информационной системе есть возможность перевода денежных средств между счетами пользователей
    // Открывать вклады, с капитализацией и без (Capitalized Interest / Simple Interest)
    // 100 12%
    // 12 меc - 112
    // 100 12%
    // 101 12%
    // 102.01 12%

    //     100
    // 1   101
    // 2   102,01
    // 3   103,0301
    // 4   104,060401
    // 5   105,101005
    // 6   106,1520151
    // 7   107,2135352
    // 8   108,2856706
    // 9   109,3685273
    // 10  110,4622125
    // 11  111,5668347
    // 12  112,682503

    // * Продумать возможность выдачи кредитов
    // Продумать использование обобщений

    // Продемонстрировать работу созданной системы

    // Банк
    // ├── Отдел работы с обычными клиентами
    // ├── Отдел работы с VIP клиентами
    // └── Отдел работы с юридическими лицами

    // Дополнительно: клиентам с хорошей кредитной историей предлагать пониженную ставку по кредиту и 
    // повышенную ставку по вкладам

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private Core core = new Core();

        public MainWindow()
        {
            InitializeComponent();
            bankList.ItemsSource = core.CreateBank();
        }

        private void MenuItem_OnClick_Debug(object sender, RoutedEventArgs e) {}

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MyBank v.0.1", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Right button menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientList_OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                ContextMenu cm = this.FindResource("CmButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
        }

        /// <summary>
        /// Refresh client's data
        /// </summary>
        void RefreshList()
        {
            // refresh clients list
            var bankDep = bankList.SelectedItem as BankDep;
            clientList.ItemsSource = (bankDep.Clients).Where(x => x != null);

            //TODO refresh clients info list
            //var clientInf = clientList.SelectedItem as Client;
            //clientInfo.ItemsSource = (bankDep.Clients).Where(x => x != null);
        }

        /// <summary>
        /// Simple deposit popup menu enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSimpleDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            pSimpDep.IsOpen = true;
        }

        /// <summary>
        /// Make simple deposit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSimpDep_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            bool result = UInt32.TryParse(amountSimpDepTextBox.Text, out uint amountSimpDeposit);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // check the client have enough money to make deposit
            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountSimpDepTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // make simple deposit
            core.MakeSimpleDeposit(currentClient, amountSimpDeposit);

            pSimpDep.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success", "Simple deposit", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// Capitalized deposit popup menu enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemCapitalizedDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            pCapDep.IsOpen = true;
        }

        /// <summary>
        /// Make capitalized deposit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemCapDep_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            bool result = UInt32.TryParse(amountCapDepTextBox.Text, out uint amountCapDeposit);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // check the client have enough money to make deposit
            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountCapDepTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // make capitalized deposit
            core.MakeCapitalizedDeposit(currentClient, amountCapDeposit);

            pCapDep.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success", "Capitalized deposit", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// Loan popup menu enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemLoan_OnClick(object sender, RoutedEventArgs e)
        {
            pLoan.IsOpen = true;
        }

        /// <summary>
        /// Get a loan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemGetLoan_OnClick(object sender, RoutedEventArgs e)
        {
            bool result = UInt32.TryParse(amountLoanTextBox.Text, out uint amountLoan);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client currentClient = clientList.SelectedItem as Client;

            // get loan
            core.GetLoan(currentClient, amountLoan);

            pLoan.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success" ,"Get loan", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }

        /// <summary>
        /// Transfer popup menu enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemTransfer_OnClick(object sender, RoutedEventArgs e)
        {
            pTransfer.IsOpen = true;
            transferTo.ItemsSource = clientList.ItemsSource;
        }

        /// <summary>
        /// Make a transfer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemMakeTransfer_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;
            Client recipient = transferTo.SelectedItem as Client;

            if (currentClient == transferTo.SelectedItem)
            {
                MessageBox.Show("You cannot make a transfer to yourself", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            bool result = UInt32.TryParse(amountTransferTextBox.Text, out uint amountTransfer);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // check the sender have enough money to make transfer
            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountTransferTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // transfer funds
            core.TransferFunds(currentClient, recipient, amountTransfer);

            pTransfer.IsOpen = false;

            RefreshList();

            MessageBox.Show("Transfer completed", "Funds transfer", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// Show deposit info popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDepInfo_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            if (currentClient.IsDeposit == Deposit.No)
            {
                MessageBox.Show("No information available", "Deposit information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            pDepInfo.IsOpen = true;

            // TODO вывод информации о депозите
            double[] months =  core.DepositInfo(currentClient);


        }

        private void MenuItemDepInfo_OnClick(object sender, RoutedEventArgs e)
        {
            pDepInfo.IsOpen = false;
        }

        /// <summary>
        /// Show clients in current bank department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BankList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bankList.SelectedItems != null)
            {
                var clients = (e.OriginalSource as ListBox).SelectedItem as BankDep;
                clientList.ItemsSource = clients.Clients;
            }
        }

        /// <summary>
        /// Show current client info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clientList.SelectedItems != null)
            {
                var client = (e.OriginalSource as ListBox).SelectedItems;
                clientInfo.ItemsSource = client;
            }
        }

        private void UsersColumnHeader_OnClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                clientList.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            clientList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
            (
                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2
            );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
