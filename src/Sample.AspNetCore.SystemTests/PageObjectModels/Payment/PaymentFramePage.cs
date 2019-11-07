﻿using Atata;
using Sample.AspNetCore.SystemTests.PageObjectModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.AspNetCore.SystemTests.PageObjectModels
{
    using _ = PaymentFramePage;

    [WaitForLoadingIndicator]
    public class PaymentFramePage : Page<_>
    {
        [ControlDefinition("div", ContainingClass = "custom-menu-card")]
        public class PayexItem : Control<_>
        {
            [FindByClass("span"), FindByIndex(0)]
            public Text<_> Name { get; private set; }

            [WaitFor(Until.Visible, TriggerEvents.BeforeAccess)]
            public Frame<_> PaymentFrame { get; private set; }
        }

        [FindById("paymentmenu-container")]
        public ControlList<PayexItem, _> PaymentMethods { get; set; }
    }
}
