using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using adWin = Autodesk.Windows;

namespace RevitAddin.UI
{
    /// <summary>
    /// Class RevitUIFactory.
    /// </summary>
    public static class RevitUIFactory
    {

        /// <summary>
        /// Adds the ribbon tab.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool AddRibbonTab(UIControlledApplication application, string tabName)
        {
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Adds the ribbon panel.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="panelName">Name of the panel.</param>
        /// <param name="addSeparator">if set to <c>true</c> [add separator].</param>
        /// <returns>RibbonPanel.</returns>
        public static RibbonPanel AddRibbonPanel(UIControlledApplication application, string tabName, string panelName, bool addSeparator)
        {
            List<RibbonPanel> panels = application.GetRibbonPanels(tabName);
            RibbonPanel panel = panels.FirstOrDefault(x => x.Name == panelName);

            if (panel == null)
            {
                panel = application.CreateRibbonPanel(tabName, panelName);
            }
            else if (addSeparator)
            {
                panel.AddSeparator();
            }

            return panel;
        }


        /// <summary>
        /// Add the ribbon tab and the plugin button to revit.
        /// </summary> 
        public static void AddRibbonButton(string btnName,
                                            string tabName,
                                            RibbonPanel panel,
                                            Type commandType,
                                            Bitmap icon32,
                                            Bitmap icon16,
                                            string tooltip
                                            )
        {
            string path = Assembly.GetExecutingAssembly().Location;
            var button = new PushButtonData("btn" + btnName.Replace(" ", ""), btnName, path, commandType.FullName)
            {
                AvailabilityClassName = typeof(CommandAvailability).FullName,
                LargeImage = Imaging.CreateBitmapSourceFromHBitmap
                (icon32.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
                Image = Imaging.CreateBitmapSourceFromHBitmap
                (icon16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
                ToolTip = tooltip
            };

            panel.AddItem(button);
            SetColor(tabName, "#FFF2CC");
        }


        /// <summary>
        /// Adds the ribbon button to a pull down button.
        /// </summary>
        /// <param name="pullDownButton">The pull down button.</param>
        /// <param name="name">The name.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="availbilityClass">The availbility class.</param>
        /// <returns>PushButton.</returns>
        public static PushButton AddRibbonButton(PulldownButton pullDownButton,
                                            string name,
                                            Type commandType,
                                            Type availbilityClass,
                                            Bitmap icon32,
                                            Bitmap icon16)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            var btn = pullDownButton.AddPushButton(new PushButtonData($"btn{name.Replace(" ", "")}", name, path, commandType.FullName));
            btn.Image = Imaging.CreateBitmapSourceFromHBitmap
                  (icon16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            btn.LargeImage = Imaging.CreateBitmapSourceFromHBitmap
                  (icon32.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            btn.AvailabilityClassName = availbilityClass.FullName;
            return btn;
        }


        /// <summary>
        /// Adds the ribbon split button.
        /// </summary>
        /// <param name="btnName">Name of the BTN.</param>
        /// <param name="panel">The panel.</param>
        /// <returns>SplitButton.</returns>
        public static SplitButton AddRibbonSplitButton(string btnName, RibbonPanel panel)
        {
            SplitButtonData splitBtnData = new SplitButtonData($"btn{btnName.Replace(" ", "")}", btnName);
            SplitButton splitBtn = panel.AddItem(splitBtnData) as SplitButton;
            return splitBtn;
        }


        /// <summary>
        /// Adds the ribbon pulldown button.
        /// </summary>
        /// <param name="btnName">Name of the BTN.</param>
        /// <param name="panel">The panel.</param>
        /// <returns>PulldownButton.</returns>
        public static PulldownButton AddRibbonPulldownButton(string btnName, RibbonPanel panel, Bitmap icon32, Bitmap icon16)
        {
            PulldownButtonData group1Data = new PulldownButtonData($"btn{btnName.Replace(" ", "")}", btnName);
            PulldownButton pullDownBtn = panel.AddItem(group1Data) as PulldownButton;
            pullDownBtn.Image = Imaging.CreateBitmapSourceFromHBitmap
                  (icon16.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            pullDownBtn.LargeImage = Imaging.CreateBitmapSourceFromHBitmap
                  (icon32.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return pullDownBtn;
        }


        /// <summary>
        /// Sets panel title background.
        /// </summary>
        /// <param name="tabName">The name of the tab, on which the all panel title background will be changed.</param>
        /// <param name="panelColor">Color of the panel.</param>
        public static void SetColor(string tabName, string panelColor)
        {
            adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;
            SolidColorBrush gradientBrush = new SolidColorBrush();

            System.Drawing.Color color = ColorTranslator.FromHtml(panelColor);
            gradientBrush.Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

            foreach (adWin.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Name == tabName)
                {
                    foreach (adWin.RibbonPanel panelInternal in tab.Panels)
                    {
                        panelInternal.CustomPanelTitleBarBackground = gradientBrush;
                    }
                    break;
                }
            }
        }
    }
}
