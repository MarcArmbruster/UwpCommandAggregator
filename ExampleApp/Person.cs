﻿namespace ExampleApp
{
    using UwpCommandAggregator;

    public class Person : BaseVm
    {
        public string Name { get; set; }

        public int Age { get; set; }

        protected override void InitCommands()
        {
        }
    }
}