using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassicAssist.Resources;
using System.Windows.Media;
using ClassicAssist.Annotations;
using ClassicAssist.Misc;
using ClassicAssist.UI.Misc;
using Newtonsoft.Json;

namespace ClassicAssist.Data.Filters.Audio
{
    public abstract class AudioEntry : INotifyPropertyChanged, IComparable<AudioEntry>
    {
        //public delegate void HotkeyChangedEventHandler(object sender, HotkeyChangedEventArgs e);

        private readonly object _childrenLock = new object();

        private ObservableCollectionEx<AudioEntry> _children = new ObservableCollectionEx<AudioEntry>();

        //private ShortcutKeys _hotkey = new ShortcutKeys();
        private bool _isCategory;

        private string _name;
        private bool _passToUo = true;

        [JsonIgnore]
        public Action<AudioEntry> Action { get; set; }

        [JsonIgnore]
        public ObservableCollectionEx<AudioEntry> Children
        {
            get
            {
                lock (_childrenLock)
                {
                    return _children;
                }
            }
            set
            {
                lock (_childrenLock)
                {
                    SetProperty(ref _children, value);
                }
            }
        }

        public virtual bool Disableable { get; set; } = true;

        //public ShortcutKeys Hotkey
        //{
        //    get => _hotkey;
        //    set
        //    {
        //        if (!Equals(value, ShortcutKeys.Default))
        //        {
        //            HotkeyManager manager = HotkeyManager.GetInstance();
        //            manager.ClearPreviousHotkey(value);
        //        }

        //        SetProperty(ref _hotkey, value);
        //        HotkeyChanged?.Invoke(this, new HotkeyChangedEventArgs(_hotkey, value));
        //    }
        //}

        [JsonIgnore]
        //public ImageSource Image =>
        //    Equals(Hotkey, ShortcutKeys.Default)
        //        ? Properties.Resources.red_circle.ToImageSource()
        //        : Properties.Resources.green_circle.ToImageSource();

        public bool IsCategory
        {
            get => _isCategory;
            set => SetProperty(ref _isCategory, value);
        }

        public virtual string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool PassToUO
        {
            get => _passToUo;
            set => SetProperty(ref _passToUo, value);
        }

        public int CompareTo(AudioEntry other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            int isCategoryComparison = _isCategory.CompareTo(other._isCategory);

            if (isCategoryComparison != 0)
            {
                return isCategoryComparison;
            }

            return string.Compare(_name, other._name, StringComparison.Ordinal);
        }

        //public event AudioChangedEventHandler AudioChanged;

        public override string ToString()
        {
            return Name;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ReSharper disable once RedundantAssignment
        public void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}