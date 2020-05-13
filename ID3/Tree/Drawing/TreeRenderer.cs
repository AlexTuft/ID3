using System;
using System.Text;

namespace C45.Tree.Drawing
{
    public static class TreeRenderer
    {
        private static int numberOfLeafNodes = 0;
        private static int maxDepth = 0;

        public static string Draw(this IDecisionTree tree)
        {
            numberOfLeafNodes = 0;
            maxDepth = 0;

            var treeView = new StringBuilder();
            DrawTree(tree, depth: 0, treeView);

            treeView.Append($"\nMax depth: {maxDepth}, Leaf nodes: {numberOfLeafNodes}\n");

            return treeView.ToString();
        }

        private static void DrawTree(IDecisionTree tree, int depth, StringBuilder treeView)
        {
            maxDepth = Math.Max(maxDepth, depth);

            if (tree is IDecisionNode decisionNode)
            {
                foreach (var child in decisionNode.Children)
                {
                    for (int _ = 0; _ < depth; _++)
                    {
                        treeView.Append("|  ");
                    }

                    treeView.Append($"{tree.Label} = {child.Key}");

                    if (child.Value is IDecisionNode)
                    {
                        treeView.Append("\n");
                        DrawTree(child.Value, depth + 1, treeView);
                    }
                    else
                    {
                        treeView.Append($" -> [{child.Value.Label}]\n");
                        numberOfLeafNodes++;
                    }
                }
            }
        }
    }
}
