//
// Raspberry Pi 3 Demoprogramm
// LED Blinklicht
// 
//
// (c) 2016 Peter Jaeger
// http://www.jaeger.to
// GitRepository @ https://github.com/pjaegerhh
// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.


// Nicht benötigte Direktiven aus Template
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Runtime.InteropServices.WindowsRuntime;
// using Windows.Foundation;
// using Windows.Foundation.Collections;
// using Windows.UI.Xaml.Data;
// using Windows.UI.Xaml.Input;
// using Windows.UI.Xaml.Navigation;

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// using Direktiven nicht im Template
// using Windows.System.Threading;
using Windows.Devices.Gpio;

//

namespace Blink_LED
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 5;                                                              // Am PI3 der GPIO Pin 5 (=GPIO5)
        private GpioPin pin;
        private GpioPinValue pinValue;
        private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);              // Rot
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);       // Grau

        public MainPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();                                                          // Timer initialisieren
            timer.Interval = TimeSpan.FromMilliseconds(500);                                        // Timer Intervall = 500 mSec
            timer.Tick += Timer_Tick;
            InitGPIO();
            if (pin != null)                                                                        // pin = null => Kein GPIO vorhanden
            {
                timer.Start();                                                                      // Timer starten
            }
        }
        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                GpioStatus.Text = "An diesem Device ist kein GPIO Controller angeschlossen.";
                return;
            }

            pin = gpio.OpenPin(LED_PIN);                                                            // LED Pin
            pinValue = GpioPinValue.High;                                                           // Value = High = AN
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioStatus.Text = "Der GPIO Pin wurde korrekt initialisiert.";

        }

        private void Timer_Tick(object sender, object e)
        {
            if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;                                                        // Value = Low = AUS
                pin.Write(pinValue);                                                                // pin.Write = Pin An- bzw Ausschalten
                LED.Fill = redBrush;                                                                // LED => XAML Page
            }
            else
            {
                pinValue = GpioPinValue.High;                                                       // Value = High = AN
                pin.Write(pinValue);                                                                // pin.Write = Pin An- bzw Ausschalten
                LED.Fill = grayBrush;                                                               // LED => XAML Page, gray
            }
        }
    }
}
