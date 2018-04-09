using Xamarin.Forms;

namespace PhantasmaMail.Effects
{
    public class MaxLinesEffect : RoutingEffect
    {
        public int MaxLines { get; set; }

        public MaxLinesEffect() : base($"io.phantasma.PhantasmaMail.{nameof(MaxLinesEffect)}") { }
    }
}
