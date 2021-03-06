using Unity.UIWidgets.ui;

namespace Unity.UIWidgets.flow {
    public class PhysicalShapeLayer : ContainerLayer {
        public PhysicalShapeLayer(
            Clip clipBehavior) {
            this._isRect = false;
            this._clip_behavior = clipBehavior;
        }

        float _elevation;
        Color _color;
        Color _shadow_color;
        float _device_pixel_ratio;
        Path _path;
        bool _isRect;
        Rect _frameRRect;
        Clip _clip_behavior;

        public Path path {
            set {
                //todo: xingwei.zhu : try to do path => rect transfer
                this._path = value;
                this._isRect = false;
                this._frameRRect = value.getBounds();
            }
        }

        public float elevation {
            set { this._elevation = value; }
        }

        public Color color {
            set { this._color = value; }
        }

        public Color shadowColor {
            set { this._shadow_color = value; }
        }

        public float devicePixelRatio {
            set { this._device_pixel_ratio = value; }
        }

        public override void preroll(PrerollContext context, Matrix3 matrix) {
            Rect child_paint_bounds = Rect.zero;
            this.prerollChildren(context, matrix, ref child_paint_bounds);

            if (this._elevation == 0) {
                this.paintBounds = this._path.getBounds();
            }
            else {
                Rect bounds = this._path.getBounds();
                //todo xingwei.zhu: outter set shadow
                //bounds.outset(20.0f, 20.0f);
                this.paintBounds = bounds;
            }
        }

        public override void paint(PaintContext context) {
            if (this._elevation != 0) {
                this.drawShadow(context.canvas, this._path, this._shadow_color, this._elevation,
                    this._color.alpha != 255, this._device_pixel_ratio);
            }

            Paint paint = new Paint {color = this._color};
            //todo: xingwei.zhu: process according to different clipBehavior, currently use antiAlias as default

            context.canvas.drawPath(this._path, paint);

            context.canvas.save();
            context.canvas.clipPath(this._path);
            this.paintChildren(context);
            context.canvas.restore();
        }


        void drawShadow(Canvas canvas, Path path, Color color, float elevation, bool transparentOccluder, float dpr) {
            //todo xingwei.zhu: to be implemented
        }
    }
}