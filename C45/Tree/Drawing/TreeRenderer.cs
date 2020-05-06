using System.Linq;
using System.Text;

namespace C45.Tree.Drawing
{
    public static class TreeRenderer
    {
        private const int NodeConnectionStartOffsetX = 2;
        private const int NodeConnectionStartOffsetY = 3;
        private const int ConnectionEndOffsetX = 5;

        public static string Draw(this IDecisionTree tree)
        {
            var canvas = new TextCanvas();
            ProcessTree(tree, canvas, 0, 0, true);
            return canvas.ToString();
        }

        private static int ProcessTree(IDecisionTree tree, TextCanvas canvas, int x, int y, bool isRootNode)
        {
            if (tree is C45TreeBuilder.DecisionTreeNode treeNode)
            {
                DrawNode(tree, x, y, canvas, isRootNode ? NodeType.Root : NodeType.Decision);

                x += NodeConnectionStartOffsetX;
                y += NodeConnectionStartOffsetY;

                // Keep current value of x so we can reset back to it later
                int connectionStartX = x;

                var children = treeNode.Children.ToList();
                for (int i = 0; i < children.Count; i++)
                {
                    bool isLastConnection = i == children.Count - 1;

                    DrawConnection(children[i].Key, x, y, canvas, isLastConnection);
                    x += children[i].Key.Length + ConnectionEndOffsetX;

                    var nextLineY = ProcessTree(children[i].Value, canvas, x, y, false);
                    x = connectionStartX;

                    if (!isLastConnection)
                    {
                        DrawConnectionLine(y + 1, nextLineY, x, canvas);
                    }

                    y = nextLineY;
                }
            }
            else
            {
                DrawNode(tree, x, y, canvas, NodeType.Leaf);
                y += NodeConnectionStartOffsetY;
            }

            return y;
        }

        private static void DrawNode(IDecisionTree node, int x, int y, TextCanvas canvas, NodeType type)
        {
            var labelLength = node.Label.Length;

            canvas.Draw(new StringBuilder()
                .Append('┌')
                .Append('─', labelLength + 2)
                .Append('┐')
                .Append('\n')
                .Append(type == NodeType.Root ? "│ " : "┤ ")
                .Append(node.Label.ToUpper())
                .Append(" │")
                .Append('\n')
                .Append('└')
                .Append('─')
                .Append(type == NodeType.Leaf ? '─' : '┬')
                .Append('─', labelLength)
                .Append('┘')
                .ToString(), x, y);
        }

        private static void DrawConnection(string connection, int x, int y, TextCanvas canvas, bool isLastConnection)
        {
            canvas.Draw(new StringBuilder()
                .Append('│')
                .Append('\n')
                .Append(isLastConnection ? "└─ " : "├─ ")
                .Append(connection.ToUpper())
                .Append(" ─")
                .ToString(), x, y);
        }

        private static void DrawConnectionLine(int y, int d, int x, TextCanvas canvas)
        {
            canvas.Draw(new StringBuilder()
                .Append("│\n", d - y)
                .ToString(), x, y + 1);
        }

        private enum NodeType
        {
            Root, Decision, Leaf
        }
    }
}
