﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    public class QrCodeImgControl : Control
    {
        private QrCode m_QrCode = new QrCode();
        private bool m_isLocked = false;
        private bool m_isFreezed = false;

        public event QrMatrixChangedEventHandler QrMatrixChanged;

        #region WBitmap
        public static readonly DependencyProperty WBitmapProperty =
            DependencyProperty.Register("WBitmap",
            typeof(WriteableBitmap),
            typeof(QrCodeImgControl),
            new UIPropertyMetadata(null, null));

        public WriteableBitmap WBitmap
        {
            get { return (WriteableBitmap)GetValue(WBitmapProperty); }
            private set { SetValue(WBitmapProperty, value); }
        }
        #endregion

        #region QrCodeWidth
        public static readonly DependencyProperty QrCodeWidthProperty =
            DependencyProperty.Register("QrCodeWidth", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(200, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int QrCodeWidth
        {
            get { return (int)GetValue(QrCodeWidthProperty); }
            set { SetValue(QrCodeWidthProperty, value); }
        }

        #endregion

        #region QrCodeHeight
        public static readonly DependencyProperty QrCodeHeightProperty =
            DependencyProperty.Register("QrCodeHeight", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(200, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int QrCodeHeight
        {
            get { return (int)GetValue(QrCodeHeightProperty); }
            set { SetValue(QrCodeHeightProperty, value); }
        }

        #endregion

        #region QuietZoneModule
        public static readonly DependencyProperty QuietZoneModuleProperty =
            DependencyProperty.Register("QuietZoneModule", typeof(QuietZoneModules), typeof(QrCodeImgControl),
            new UIPropertyMetadata(QuietZoneModules.Two, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public QuietZoneModules QuietZoneModule
        {
            get { return (QuietZoneModules)GetValue(QuietZoneModuleProperty); }
            set { SetValue(QuietZoneModuleProperty, value); }
        }

        #endregion

        #region LightColor
        public static readonly DependencyProperty LightColorProperty =
            DependencyProperty.Register("LightColor", typeof(Color), typeof(QrCodeImgControl),
            new UIPropertyMetadata(Colors.White, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color LightColor
        {
            get { return (Color)GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }

        #endregion

        #region DarkColor
        public static readonly DependencyProperty DarkColorProperty =
            DependencyProperty.Register("DarkColor", typeof(Color), typeof(QrCodeImgControl),
            new UIPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color DarkColor
        {
            get { return (Color)GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }

        #endregion

        #region ErrorCorrectionLevel
        public static readonly DependencyProperty ErrorCorrectLevelProperty =
            DependencyProperty.Register("ErrorCorrectLevel", typeof(ErrorCorrectionLevel), typeof(QrCodeImgControl),
            new UIPropertyMetadata(ErrorCorrectionLevel.M, new PropertyChangedCallback(OnQrValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return (ErrorCorrectionLevel)GetValue(ErrorCorrectLevelProperty); }
            set { SetValue(ErrorCorrectLevelProperty, value); }
        }
        #endregion

        #region Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(QrCodeImgControl),
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnQrValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region IsGrayImage
        public static readonly DependencyProperty IsGrayImageProperty =
            DependencyProperty.Register("IsGrayImage", typeof(bool), typeof(QrCodeImgControl),
            new UIPropertyMetadata(true, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public bool IsGrayImage
        {
            get { return (bool)GetValue(IsGrayImageProperty); }
            set { SetValue(IsGrayImageProperty, value); }
        }

        #endregion

        #region DpiX
        public static readonly DependencyProperty DpiXProperty =
            DependencyProperty.Register("DpiX", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(96, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int DpiX
        {
            get { return (int)GetValue(DpiXProperty); }
            set { SetValue(DpiXProperty, value); }
        }

        #endregion

        #region DpiY
        public static readonly DependencyProperty DpiYProperty =
            DependencyProperty.Register("DpiY", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(96, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int DpiY
        {
            get { return (int)GetValue(DpiYProperty); }
            set { SetValue(DpiYProperty, value); }
        }

        #endregion

        static QrCodeImgControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(typeof(QrCodeImgControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalAlignmentProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        public QrCodeImgControl()
        {
            this.EncodeAndUpdateBitmap();
        }

        #region ReDraw Bitmap, Update Qr Cache

        private void CreateBitmap()
        {
            WBitmap = null;
            if (IsGrayImage)
                WBitmap = new WriteableBitmap(QrCodeWidth, QrCodeHeight, DpiX, DpiY, PixelFormats.Gray8, null);
            else
                WBitmap = new WriteableBitmap(QrCodeWidth, QrCodeHeight, DpiX, DpiY, PixelFormats.Pbgra32, null);
        }

        private void UpdateSource()
        {
            if (WBitmap == null)
                this.CreateBitmap();
            else
            {
                if (WBitmap.PixelWidth != QrCodeWidth ||
                    WBitmap.PixelHeight != QrCodeHeight ||
                    WBitmap.DpiX != DpiX ||
                    WBitmap.DpiY != DpiY ||
                    !this.isColorSettingCorrect())
                    this.CreateBitmap();
            }

            if (QrCodeWidth != 0 && QrCodeHeight != 0)
                WBitmap.Clear(LightColor);

            if (m_QrCode.Matrix != null)
            {
                int offsetX, offsetY, width;
                if (QrCodeWidth <= QrCodeHeight)
                {
                    offsetX = 0;
                    offsetY = (QrCodeHeight - QrCodeWidth) / 2;
                    width = QrCodeWidth;
                }
                else
                {
                    offsetX = (QrCodeWidth - QrCodeHeight) / 2;
                    offsetY = 0;
                    width = QrCodeHeight;
                }

                new WriteableBitmapRenderer(new FixedCodeSize(width, QuietZoneModule), DarkColor, LightColor).DrawDarkModule(WBitmap, m_QrCode.Matrix, offsetX, offsetY);
            }
        }

        private void UpdateQrCodeCache()
        {
            new QrEncoder(ErrorCorrectLevel).TryEncode(this.Text, out m_QrCode);
            OnQrMatrixChanged(new EventArgs());
        }

        #endregion

        private bool isColorSettingCorrect()
        {
            if (IsGrayImage)
            {
                if (WBitmap.Format != PixelFormats.Gray8)
                    return false;
                else
                    return true;
            }
            else
            {
                if (WBitmap.Format != PixelFormats.Pbgra32)
                    return false;
                else
                    return true;
            }
        }

        #region Event method

        public static void OnVisualValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl)d).UpdateBitmap();
        }

        /// <summary>
        /// Encode and Update bitmap when ErrorCorrectlevel or Text changed. 
        /// </summary>
        public static void OnQrValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl)d).EncodeAndUpdateBitmap();
        }

        #endregion

        #region Update method

        public void EncodeAndUpdateBitmap()
        {
            if (!IsLocked)
            {
                this.UpdateQrCodeCache();
                this.UpdateBitmap();
            }
        }

        public void UpdateBitmap()
        {
            if(!IsFreezed)
                this.UpdateSource();
        }

        #endregion

        #region Lock Freeze

        /// <summary>
        /// If Class is locked, it won't update QrMatrix cache.
        /// </summary>
        public void Lock()
        {
            m_isLocked = true;
        }

        /// <summary>
        /// Unlock class will cause class to update QrMatrix Cache and redraw bitmap. 
        /// </summary>
        public void Unlock()
        {
            m_isLocked = false;
            this.EncodeAndUpdateBitmap();
        }

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        public bool IsLocked
        { get { return m_isLocked; } }

        /// <summary>
        /// Freeze Class, Any value change to Brush, QuietZoneModule won't cause immediately redraw bitmap. 
        /// </summary>
        public void Freeze()
        { m_isFreezed = true; }

        /// <summary>
        /// Unfreeze and redraw immediately. 
        /// </summary>
        public void UnFreeze()
        {
            m_isFreezed = false;
            this.UpdateBitmap();
        }

        /// <summary>
        /// Return whether if class is freezed. 
        /// </summary>
        public bool IsFreezed
        { get { return m_isFreezed; } }

        #endregion

        /// <summary>
        /// QrCode matrix cache updated.
        /// </summary>
        protected virtual void OnQrMatrixChanged(EventArgs e)
        {
            if (QrMatrixChanged != null)
                QrMatrixChanged(this, e);
        }

        /// <summary>
        /// Get Qr BitMatrix as two dimentional bool array.
        /// </summary>
        /// <returns>null if matrix is null, else full matrix</returns>
        public bool[,] GetQrMatrix()
        {
            if (m_QrCode.Matrix == null)
                return null;
            else
            {
                bool[,] clone = new bool[m_QrCode.Matrix.Width, m_QrCode.Matrix.Width];
                BitMatrix matrix = m_QrCode.Matrix;
                for (int x = 0; x < matrix.Width; x++)
                {
                    for (int y = 0; y < matrix.Width; y++)
                    {
                        clone[x, y] = matrix[x, y];
                    }
                }
                return clone;
            }
        }

    }
}
