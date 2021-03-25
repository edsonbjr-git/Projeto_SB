using Xamarin.Forms;

namespace BalizaFacil.UI
{
    public class SocialMediaButton : RelativeLayout
    {
        private View button;
        public View Button
        {
            get => button;
            set
            {
                button = value;
                
                this.Children.Add(button, Constraint.RelativeToParent(parent => 0), Constraint.RelativeToParent(parent => 0), Constraint.RelativeToParent(parent => parent.Width), Constraint.RelativeToParent(parent => parent.Height));
            }
        }
        public Frame InnerFrame;
        private View icon;
        public View Icon
        {
            get => icon;
            set
            {
                icon = value;
                InnerFrame.Content = icon;

                this.Children.Add(InnerFrame, Constraint.RelativeToParent(parent => 0), Constraint.RelativeToParent(parent => 0), Constraint.RelativeToParent(parent => parent.Width), 
                    Constraint.RelativeToParent(parent => {

                        return parent.Height;
                    }));

            }
        }

        public SocialMediaButton()
        {
            InnerFrame = new Frame()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                OutlineColor = Color.Transparent
            };
        }


    }
}
