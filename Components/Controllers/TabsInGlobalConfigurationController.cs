using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace forDNN.Modules.UniversalAutosave
{
    public class TabsInGlobalConfigurationController
    {
        public static void TabsInGlobalConfiguration_Add(int ConfigurationID, int TabID)
        {
            DataProvider.Instance().TabsInGlobalConfiguration_Add(ConfigurationID, TabID);
        }
    }
}