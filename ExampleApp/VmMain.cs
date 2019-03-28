namespace ExampleApp
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using UwpCommandAggregator;

    public class VmMain : UwpCommandAggregator.BaseVm
    {
        private bool isAllowed;
        public bool IsAllowed
        {
            get { return isAllowed; }
            set
            {
                isAllowed = value;
                this.NotifyPropertyChanged("IsAllowed");
                //this.RaisePropertyChanged("CmdAgg");  --> not necessary due to AutoTriggerCommandNotification = true
            }
        }
     
        /// <summary>
        /// The can save2 value.
        /// </summary>
        private bool canSave2;

        /// <summary>
        /// Input string to demonstrate Dependency Attribute.
        /// </summary>
        private string stringInput;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can save1.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can save1; otherwise, <c>false</c>.
        /// </value>       
        public bool CanSave1
        {
            // using NO private field -> using automatic values storage (base class).

            get => this.GetPropertyValue<bool>();
            set => this.SetPropertyValue<bool>(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can save2.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can save2; otherwise, <c>false</c>.
        /// </value>       
        public bool CanSave2
        {
            get => this.canSave2;
            set => this.SetPropertyValue(ref this.canSave2, value, ()=> { }, () => this.NotifyPropertyChanged(nameof(CmdAgg)));
        }

        public string StringInput
        {
            get => this.stringInput;
            set => this.SetPropertyValue(ref this.stringInput, value);
        }

        [DependsOn(nameof(StringInput))]
        public string StringOutput
        {
            get => $"Length: {this?.StringInput?.Length ?? 0}";
        }

        protected override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("HelloCommand", p1 => SayHelloAsync(p1), p2 => CanSayHello(p2));

            // Adding a hierarchy command
            ICommand save1Cmd = new RelayCommand(new Action<object>(p1 => ShowMessageAsync("Save 1 called")), new Predicate<object>(p2 => this.CanSave1));
            ICommand save2Cmd = new RelayCommand(new Action<object>(p1 => ShowMessageAsync("Save 2 called")), new Predicate<object>(p2 => this.CanSave2));
            this.CmdAgg.AddOrSetCommand("SaveCmd1", save1Cmd);
            this.CmdAgg.AddOrSetCommand("SaveCmd2", save2Cmd);

            HierarchyCommand saveAllCmd = new HierarchyCommand(
                p1 => ShowMessageAsync("Save All called"),
                null,
                HierarchyExecuteStrategy.MasterCommandOnly,
                HierarchyCanExecuteStrategy.DependsOnAllChilds);

            saveAllCmd.AddChildsCommand(new List<ICommand> { save1Cmd, save2Cmd });
            this.CmdAgg.AddOrSetCommand("SaveAll", saveAllCmd);
        }

        private async void ShowMessageAsync(string text)
        {
            Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog(text, "Example");
            var result = await dlg.ShowAsync();
        }

        private async void SayHelloAsync(object cmdParameter)
        {
            Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog("Hello", "Hello Message");
            var result = await dlg.ShowAsync();
        }

        private bool CanSayHello(object cmdParameter)
        {
            return this.IsAllowed;
        }

        public async void OnClick(object sender, EventArgs eventArgs)
        {
            Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog("Clicked", "Click Example");
            var result = await dlg.ShowAsync();

        }
    }
}