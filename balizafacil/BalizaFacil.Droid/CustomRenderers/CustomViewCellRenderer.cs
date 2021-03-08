using Android.Graphics.Drawables;
using Android.Views;
using BalizaFacil.Droid.CustomRenderers;
using BalizaFacil.UI;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace BalizaFacil.Droid.CustomRenderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        private Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Android.Content.Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);
            _selected = false;
            _unselectedBackground = _cellCore.Background;

            return _cellCore;
        }
        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                _selected = !_selected;

                var cell = this.Cell as CustomViewCell;
                if (cell.label == null)
                    return;

                cell.label.TextColor = _selected ? cell.SelectedTextColor : cell.NormalTextColor;
                _cellCore.SetBackgroundColor(Color.White.ToAndroid());
            }
            else
            {
                _cellCore.SetBackground(_unselectedBackground);
            }
        }
    }

}