﻿using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class SelectorNode : BaseNode {

        public IList<ElementNode> Elements { get; } = new List<ElementNode>();

        public IList<CommentNode> Comments { get; } = new List<CommentNode>();

        public static SelectorNode Parse(SscsParserContext context) {
            var res = new SelectorNode();
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        res.Comments.Add(new CommentNode(preview));
                        break;
                    case TokenType.Literal:
                    case TokenType.Hash:
                        var el = ElementNode.Parse(context);
                        res.Elements.Add(el);
                        break;
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        break;
                    case TokenType.OpenCurlyBracket:
                        stop = true;
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            return res;
        }

    }
}
