using System.Text;

namespace C45.Tree.Drawing
{
    public static class StringBuilderHelper
    {
        public static StringBuilder Append(this StringBuilder sb, string s, int count)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append(s);
            }
            return sb;
        }
    }
}
