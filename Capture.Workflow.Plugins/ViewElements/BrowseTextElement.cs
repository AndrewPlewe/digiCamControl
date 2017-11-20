﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using CameraControl.Devices;
using Capture.Workflow.Core.Classes;
using Capture.Workflow.Core.Classes.Attributes;
using Capture.Workflow.Core.Interface;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace Capture.Workflow.Plugins.ViewElements
{
    [Description("")]
    [PluginType(PluginType.ViewElement)]
    [DisplayName("BrowseFolder")]
    public class BrowseTextElement: IViewElementPlugin
    {
        public string Name { get; set; }
        public WorkFlowViewElement CreateElement(WorkFlowView view)
        {
            WorkFlowViewElement element = new WorkFlowViewElement();
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "(Name)",
                PropertyType = CustomPropertyType.String
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Caption",
                PropertyType = CustomPropertyType.String
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Position",
                PropertyType = CustomPropertyType.ValueList,
                ValueList = view.Instance.GetPositions(),
                Value = view.Instance.GetPositions()[0]
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Width",
                PropertyType = CustomPropertyType.Number,
                RangeMin = 0,
                RangeMax = 9999,
                Value = "150"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "LabelWidth",
                PropertyType = CustomPropertyType.Number,
                RangeMin = 0,
                RangeMax = 9999,
                Value = "150"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Height",
                PropertyType = CustomPropertyType.Number,
                RangeMin = 0,
                RangeMax = 9999,
                Value = "35"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Orientation",
                PropertyType = CustomPropertyType.ValueList,
                ValueList = new List<string>() { "Horizontal", "Vertical" },
                Value = "Horizontal"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Margins",
                PropertyType = CustomPropertyType.Number,
                RangeMin = 0,
                RangeMax = 9999,
                Value = "5"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "FontSize",
                PropertyType = CustomPropertyType.Number,
                RangeMin = 6,
                RangeMax = 400,
                Value = "15"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "BackgroundColor",
                PropertyType = CustomPropertyType.Color,
                Value = "Transparent"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "ForegroundColor",
                PropertyType = CustomPropertyType.Color,
                Value = "Transparent"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "ButtonText",
                PropertyType = CustomPropertyType.String,
                Value = "Browse"
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "Variable",
                PropertyType = CustomPropertyType.Variable,
                Value = ""
            });
            element.Properties.Items.Add(new CustomProperty()
            {
                Name = "File",
                PropertyType = CustomPropertyType.File,
                Value = ""
            });
            return element;
        }

        public FrameworkElement GetControl(WorkFlowViewElement viewElement, Context context)
        {
            var textBox = new TextBox()
            {
                FontSize = viewElement.Properties["FontSize"].ToInt(context),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            viewElement.SetSize(textBox,context);

            textBox.DataContext = viewElement.Parent.Parent.Variables[viewElement.Properties["Variable"].Value];
            textBox.SetBinding(TextBox.TextProperty, "Value");

            if (viewElement.Properties["BackgroundColor"].Value != "Transparent" && viewElement.Properties["BackgroundColor"].Value != "#00FFFFFF")
                textBox.Background =
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(viewElement.Properties["BackgroundColor"].Value));

            if (viewElement.Properties["ForegroundColor"].Value != "Transparent" && viewElement.Properties["ForegroundColor"].Value != "#00FFFFFF")
                textBox.Foreground =
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(viewElement.Properties["ForegroundColor"].Value));

            var label = new System.Windows.Controls.Label()
            {
                Height = viewElement.Properties["Height"].ToInt(context),
                Content = viewElement.Properties["Caption"].Value,
                Margin = new Thickness(viewElement.Properties["Margins"].ToInt(context)),
                FontSize = viewElement.Properties["FontSize"].ToInt(context),
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            if (viewElement.Properties["LabelWidth"].ToInt(context) > 0)
                label.Width = viewElement.Properties["LabelWidth"].ToInt(context);


            if (viewElement.Properties["ForegroundColor"].Value != "Transparent" && viewElement.Properties["ForegroundColor"].Value != "#00FFFFFF")
                label.Foreground =
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(viewElement.Properties["ForegroundColor"].Value));

            var button = new System.Windows.Controls.Button()
            {
                Height = viewElement.Properties["Height"].ToInt(context),
                Content = viewElement.Properties["ButtonText"].Value,
                Margin = new Thickness(viewElement.Properties["Margins"].ToInt(context)),
                FontSize = viewElement.Properties["FontSize"].ToInt(context),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            if (viewElement.Properties["Orientation"].Value == "Vertical")
                button.HorizontalAlignment = HorizontalAlignment.Stretch;

            button.Click += (sender, e) =>
            {
                try
                {
                    FolderBrowserDialog browserDialog = new FolderBrowserDialog();
                    if (viewElement.Parent.Parent.Variables[viewElement.Properties["Variable"].Value].Value != null)
                        browserDialog.SelectedPath =
                            viewElement.Parent.Parent.Variables[viewElement.Properties["Variable"].Value].Value;
                    if (browserDialog.ShowDialog() == DialogResult.OK)
                    {
                        viewElement.Parent.Parent.Variables[viewElement.Properties["Variable"].Value].Value =
                            browserDialog.SelectedPath;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unable to browse folder", ex);
                }
            };
            var stackpanel = new StackPanel();
            stackpanel.Children.Add(label);
            stackpanel.Children.Add(textBox);
            stackpanel.Children.Add(button);
            stackpanel.Orientation = viewElement.Properties["Orientation"].Value == "Horizontal" ? Orientation.Horizontal : Orientation.Vertical;
            return stackpanel;
        }
    }
}
