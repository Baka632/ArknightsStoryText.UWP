using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArknightsStoryText.UWP.ViewModels
{
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知运行时属性已经发生更改
        /// </summary>
        /// <param name="propertyName">发生更改的属性名称,其填充是自动完成的</param>
        public void OnPropertiesChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 通知运行时属性已经发生更改
        /// </summary>
        /// <param name="obj">发生更改的属性所属的对象</param>
        /// <param name="propertyName">发生更改的属性名称,其填充是自动完成的</param>
        public void OnPropertiesChanged(object obj, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
        }
    }
}
