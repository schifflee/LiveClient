using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using MahApps.Metro.Controls;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {

        private readonly SettingsViewModel _settingsViewModel;
        private AudioFileReader audioFileReader;
        private IAudioDevice _debugAudioDevice;
        private IWavePlayer waveOut;

        private int _oldLeftChannelLoudness;
        private bool _isPlayback;
        public SettingsWindow()
        {
            InitializeComponent();
        }
        public SettingsWindow(SettingsViewModel settingsViewModel, RuntimeState runtimeState)
            : this()
        {
            this.DataContext = _settingsViewModel = settingsViewModel;
            _settingsViewModel.SaveBtnIsEnable = !(runtimeState.IsLiving || runtimeState.IsRecording);
        }

        private void MicrophonPlayBtn_Click(object sender, RoutedEventArgs e)
        {
            MicrophonPlayBtn.Visibility = Visibility.Hidden;
            MicrophonCloseBtn.Visibility = Visibility.Visible;

            _debugAudioDevice = _settingsViewModel.AduioDeviceList.First(item => item.ID == _settingsViewModel.DebugAduioDevice.ID);
            _debugAudioDevice.PushingData += AudioDevice_PushingData;

        }

        private void MicrophonCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_debugAudioDevice != null)
            {
                _debugAudioDevice.PushingData -= AudioDevice_PushingData;
            }
            MicrophoneLoudness.Value = 0;
            MicrophonPlayBtn.Visibility = Visibility.Visible;
            MicrophonCloseBtn.Visibility = Visibility.Hidden;

        }
        private void AudioDevice_PushingData(AudioDeviceDataContext context)
        {
            int leftChannelLoudness = 0;
            int rightChannelLoudness = 0;
            _debugAudioDevice.GetAudioLevel(context.Data, context.DataLength, ref leftChannelLoudness, ref rightChannelLoudness);
            if (leftChannelLoudness != _oldLeftChannelLoudness)
            {
                _oldLeftChannelLoudness = leftChannelLoudness;
                Dispatcher.Invoke(() =>
                {
                    MicrophoneLoudness.Value = leftChannelLoudness;
                });
            }
        }

        private void PlaySoundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlayback)
            {
                return;
            }
            _isPlayback = true;
            try
            {
                CreateWaveOut();
            }
            catch (Exception driverCreateException)
            {
                _isPlayback = false;
                MessageBox.Show(String.Format("{0}", driverCreateException.Message));
                return;
            }

            ISampleProvider sampleProvider;

            try
            {
                sampleProvider = CreateInputStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/sound.mp3"));
            }
            catch (Exception createException)
            {
                _isPlayback = false;
                MessageBox.Show(String.Format("{0}", createException.Message), "Error Loading File");
                return;
            }
            try
            {
                waveOut.Init(sampleProvider);
            }
            catch (Exception initException)
            {
                _isPlayback = false;
                MessageBox.Show(String.Format("{0}", initException.Message), "Error Initializing Output");
                return;
            }
            waveOut.Play();
        }
        private ISampleProvider CreateInputStream(string fileName)
        {
            audioFileReader = new AudioFileReader(fileName);

            var sampleChannel = new SampleChannel(audioFileReader, true);
            //sampleChannel.PreVolumeMeter += OnPreVolumeMeter;
            //setVolumeDelegate = vol => sampleChannel.Volume = vol;
            var postVolumeMeter = new MeteringSampleProvider(sampleChannel);
            postVolumeMeter.StreamVolume += OnPostVolumeMeter;

            return postVolumeMeter;
        }
        private void OnPostVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            SoundLoudness.Value = e.MaxSampleValues[0];
            //e.MaxSampleValues[1];
        }

        private void CreateWaveOut()
        {
            CloseWaveOut();
            WaveOut outputDevice = new WaveOut();
            outputDevice.DeviceNumber = (int)SpeakerCombobox.SelectedValue;
            outputDevice.DesiredLatency = 300;
            waveOut = outputDevice;
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped; ;
        }
        private void CloseWaveOut()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }
        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            _isPlayback = false;
            SoundLoudness.Value = 0;
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message, "Playback Device Error");
            }
            if (audioFileReader != null)
            {
                audioFileReader.Position = 0;
            }
        }

        private void OpenCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseCameraBtn.Visibility = Visibility.Visible;
            PlayerPanel.Visibility = Visibility.Visible;
            CameraCombobox.IsEnabled = true;
            OpenVideoDevice();
        }

        private void CloseCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseCameraBtn.Visibility = Visibility.Hidden;
            PlayerPanel.Visibility = Visibility.Hidden;
            CameraCombobox.IsEnabled = false;
            MsPlayerControl.CloseDevice();
        }

        private void CameraCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerPanel.Visibility == Visibility.Visible)
            {
                OpenVideoDevice();
            }
        }
        private void OpenVideoDevice()
        {
            MsPlayerControl.OpenDevice(_settingsViewModel.SelectedVideoDevice.OwnerVideoDevice);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _settingsViewModel.Save();
            Close();
        }

        private void ChangeFolerBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel) return;
            _settingsViewModel.RecFileSavePath = m_Dialog.SelectedPath.Trim();
        }

        private void OpenFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(_settingsViewModel.RecFileSavePath))
                Process.Start("explorer.exe", _settingsViewModel.RecFileSavePath);
            else
                MessageBox.Show("打开失败,目录已被删除", "系统提示");
        }
    }
}
