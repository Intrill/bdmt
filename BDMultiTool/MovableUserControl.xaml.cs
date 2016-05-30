﻿using BDMultiTool.Macros;
using BDMultiTool.Utilities.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BDMultiTool {
    /// <summary>
    /// Interaction logic for MovableUserControl.xaml
    /// </summary>
    public partial class MovableUserControl : UserControl {
        private static int minSize = 100;
        private bool windowEventInitialized;
        private Point oldMousePosition;
        private Point anchorPoint;
        private Point currentMousePosition;
        public String lockedCollider { private set; get; }
        private Grid parent;

        public MovableUserControl(Grid parent) {
            InitializeComponent();
            anchorPoint = new Point();
            this.parent = parent;
        }

        public void setTitle(String title) {
            this.subWindowTitle.Content = title;
        }

        public void setGridContent(UserControl userControl) {
            this.contentGrid.Children.Add(userControl);
        }

        private void GeneralRectangleMouseDown(object sender, MouseButtonEventArgs e) {
            if (!windowEventInitialized) {
                oldMousePosition = PointToScreen(e.GetPosition(this));
                lockedCollider = (sender as Rectangle).Name;
                (sender as Rectangle).CaptureMouse();
            }
            windowEventInitialized = true;
        }

        public void enableToggle(bool value) {
            if (value) {
                lockedCollider = "toggle";
            } else {
                lockedCollider = "";
            }

        }

        private void GeneralRectangleMouseMove(object sender, MouseEventArgs e) {
            if (windowEventInitialized) {
                currentMousePosition = PointToScreen(e.GetPosition(this));
                double distanceX = currentMousePosition.X - oldMousePosition.X;
                double distanceY = currentMousePosition.Y - oldMousePosition.Y;
                oldMousePosition = PointToScreen(e.GetPosition(this));
                switch (lockedCollider) {
                    case "TopBorder": {
                            if ((this.Height - distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height - distanceY;

                                translateForHeightResize(distanceY);
                            }
                        }
                        break;
                    case "BottomBorder": {
                            if ((this.Height + distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height + distanceY;

                                translateForHeightResize(distanceY);
                            }

                        }
                        break;
                    case "LeftBorder": {
                            if ((this.Width - distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width - distanceX;

                                translateForWidthResize(distanceX);
                            }


                        }
                        break;
                    case "RightBorder": {
                            if ((this.Width + distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width + distanceX;

                                translateForWidthResize(distanceX);
                            }

                        }
                        break;
                    case "BottomLeftBorder": {
                            if ((this.Width - distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width - distanceX;

                                translateForWidthResize(distanceX);
                            }

                            if ((this.Height + distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height + distanceY;

                                translateForHeightResize(distanceY);
                            }
                        }
                        break;
                    case "BottomRightBorder": {
                            if ((this.Width + distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width + distanceX;

                                translateForWidthResize(distanceX);
                            }

                            if ((this.Height + distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height + distanceY;

                                translateForHeightResize(distanceY);
                            }
                        }
                        break;
                    case "TopLeftBorder": {
                            if ((this.Width - distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width - distanceX;

                                translateForWidthResize(distanceX);
                            }

                            if ((this.Height - distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height - distanceY;

                                translateForHeightResize(distanceY);
                            }
                        }
                        break;
                    case "TopRightBorder": {
                            if ((this.Width + distanceX) < minSize) {
                                this.Width = minSize;
                            } else {
                                this.Width = this.Width + distanceX;

                                translateForWidthResize(distanceX);
                            }

                            if ((this.Height - distanceY) < minSize) {
                                this.Height = minSize;
                            } else {
                                this.Height = this.Height - distanceY;

                                translateForHeightResize(distanceY);
                            }
                        }
                        break;
                    case "DragHighlighter": {
                            translateBy(distanceX, distanceY);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void translateBy(double distanceX, double distanceY) {
            TranslateTransform transform = new TranslateTransform();
            transform.X = distanceX + anchorPoint.X;
            transform.Y = distanceY + anchorPoint.Y;
            this.RenderTransform = transform;

            anchorPoint.X = transform.X;
            anchorPoint.Y = transform.Y;
        }

        private void translateForHeightResize(double distanceY) {
            TranslateTransform transform = new TranslateTransform();
            transform.X = anchorPoint.X;
            transform.Y = distanceY / 2 + anchorPoint.Y;
            this.RenderTransform = transform;

            anchorPoint.X = transform.X;
            anchorPoint.Y = transform.Y;
        }

        private void translateForWidthResize(double distanceX) {
            TranslateTransform transform = new TranslateTransform();
            transform.X = distanceX / 2 + anchorPoint.X;
            transform.Y = anchorPoint.Y;
            this.RenderTransform = transform;

            anchorPoint.X = transform.X;
            anchorPoint.Y = transform.Y;
        }

        private void GeneralRectangleMouseUp(object sender, MouseButtonEventArgs e) {
            windowEventInitialized = false;
            lockedCollider = "";
            (sender as Rectangle).ReleaseMouseCapture();
        }

        private void RectangleHighlightMouseEnter(object sender, MouseEventArgs e) {
            (sender as Rectangle).Opacity = 0.8;
        }

        private void RectangleHighlightMouseLeave(object sender, MouseEventArgs e) {
            (sender as Rectangle).Opacity = 0.5;
        }

        private void closeButton_MouseUp(object sender, MouseButtonEventArgs e) {
            this.Visibility = Visibility.Hidden;
            //parent.Children.Remove(this);
        }

    }
}
