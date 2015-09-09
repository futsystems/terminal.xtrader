﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;
using Telerik.WinControls;


namespace TradingLib.TraderControl
{
    public class FutsPriceArrowButtonElement: RadArrowButtonElement
    {
        // Methods
        static FutsPriceArrowButtonElement()
        {
            ItemStateManagerFactoryRegistry.AddStateManagerFactory(new RadCalculatorArrowButtonElementStateManagerFactory(RadCalculatorDropDownElement.IsDropDownShownProperty), typeof(RadCalculatorArrowButtonElement));
        }
    }

   
}
