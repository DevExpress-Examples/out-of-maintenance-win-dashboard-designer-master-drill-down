using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.Collections.Generic;
using System.Linq;

namespace WinForms_Dashboard_Drill_Down {
    public class MasterDrillDownModule {
        static string MFDPropertyName = "MasterDrillDown";
        static string IgnoreMFDPropertyName = "IgnoreMasterDrillDown";

        readonly DashboardDesigner designer;
        readonly RibbonControl ribbon;
        BarCheckItem mfdBarItem;
        BarCheckItem ignoreMfdBarItem;

        public MasterDrillDownModule(DashboardDesigner designer) {
            this.designer = designer;
            designer.DrillDownPerformed += OnDrillDownPerformed;
            designer.DrillUpPerformed += OnDrillUpPerformed;
            designer.DashboardItemSelected += OnDashboardItemSelected;
            designer.DashboardCustomPropertyChanged += OnCustomPropertyChanged;

            ribbon = designer.Ribbon;
            CreateBarItems(DashboardBarItemCategory.ChartTools, DashboardBarItemCategory.GridTools);
        }
        void CreateBarItems(params DashboardBarItemCategory[] categories) {
            mfdBarItem = new BarCheckItem(ribbon.Manager, false);
            mfdBarItem.Caption = MFDPropertyName;
            mfdBarItem.ItemClick += OnMFDButtonClick;
            ignoreMfdBarItem = new BarCheckItem(ribbon.Manager, false);
            ignoreMfdBarItem.Caption = IgnoreMFDPropertyName;
            ignoreMfdBarItem.ItemClick += OnIgnoreMFDBarItemClick;

            foreach(DashboardBarItemCategory category in categories) {
                RibbonPage page = ribbon.GetDashboardRibbonPage(category, DashboardRibbonPage.Data);
                RibbonPageGroup group = page.GetGroupByName("Drill-Down");
                if(group == null) {
                    group = new RibbonPageGroup("Drill-Down") { Name = "Drill-Down" };
                    page.Groups.Add(group);
                }
                group.ItemLinks.Add(mfdBarItem);
                group.ItemLinks.Add(ignoreMfdBarItem);
            }
        }
        void OnDrillDownPerformed(object sender, DrillActionEventArgs e) {
            SyncronizeData(e);
        }
        void OnDrillUpPerformed(object sender, DrillActionEventArgs e) {
            SyncronizeData(e);
        }
        void SyncronizeData(DrillActionEventArgs e) {
            if(!GetCustomPropertyValue(e.DashboardItemName, MFDPropertyName))
                return;

            List<object> drillDownValues = new List<object>();
            if(e.DrillDownLevel > 0) {
                for(int i = 0; i < e.DrillDownLevel; i++)
                    drillDownValues.Add(e.Values[0].DataSet.GetValue(0, i));
            }

            DashboardState state = new DashboardState();
            foreach(var item in designer.Dashboard.Items.OfType<DataDashboardItem>().Where(i => !GetCustomPropertyValue(i.ComponentName, IgnoreMFDPropertyName)))
                state.Items.Add(new DashboardItemState(item.ComponentName) { DrillDownValues = drillDownValues });
            designer.SetDashboardState(state);
        }
        void OnCustomPropertyChanged(object sender, CustomPropertyChangedEventArgs e) {
            if(e.Name == MFDPropertyName)
                UpdateMasterDrillDownBarItem();
            if(e.Name == IgnoreMFDPropertyName)
                UpdateIgnoreMasterDrillDownBarItem();
        }
        void OnDashboardItemSelected(object sender, DashboardItemSelectedEventArgs e) {
            UpdateMasterDrillDownBarItem();
            UpdateIgnoreMasterDrillDownBarItem();
        }
        void OnMFDButtonClick(object sender, ItemClickEventArgs e) {
            var newValue = !GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, MFDPropertyName);
            designer.AddToHistory(new CustomPropertyHistoryItem(designer.SelectedDashboardItem, MFDPropertyName, newValue.ToString(), string.Format("{0} Changed", MFDPropertyName)));
        }

        void OnIgnoreMFDBarItemClick(object sender, ItemClickEventArgs e) {
            var newValue = !GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, IgnoreMFDPropertyName);
            designer.AddToHistory(new CustomPropertyHistoryItem(designer.SelectedDashboardItem, IgnoreMFDPropertyName, newValue.ToString(), string.Format("{0} Changed", IgnoreMFDPropertyName)));
        }

        void UpdateMasterDrillDownBarItem() {
            mfdBarItem.Checked = GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, MFDPropertyName);
            ignoreMfdBarItem.Enabled = !mfdBarItem.Checked;
        }
        void UpdateIgnoreMasterDrillDownBarItem() {
            ignoreMfdBarItem.Checked = GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, IgnoreMFDPropertyName);
            mfdBarItem.Enabled = !ignoreMfdBarItem.Checked;
        }
        bool GetCustomPropertyValue(string itemName, string propertyName) {
            DashboardItem dashboardItem = designer.Dashboard.Items[itemName];
            return dashboardItem != null ? dashboardItem.CustomProperties.GetValue<bool>(propertyName) : false;
        }
    }
}
