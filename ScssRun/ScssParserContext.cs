using System;
using System.Collections.Generic;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun {
    public class ScssParserContext {

        private readonly Stack<RuleSetNode> _ruleSets = new Stack<RuleSetNode>();
        public TokensQueue Tokens { get; }

        public RuleSetNode CurrentRuleSet => _ruleSets.Count > 0 ? _ruleSets.Peek() : null;

        //private readonly Stack<IList<ScssDeclarationNode>> _declarationOwners = new Stack<IList<ScssDeclarationNode>>();

        public ScssParserContext(TokensQueue tokens) {
            Tokens = tokens;
        }

        //remove?
        public void PushRule(ScssDeclarationNode rule) {
            var set = CurrentRuleSet;
            if (set == null) {
                throw new TokenException("selector expected", Tokens.LastReadToken);
            }
        }

        public void PopRule() {
            
        }

        public void PushRuleSet(RuleSetNode ruleSet) {
            _ruleSets.Push(ruleSet);
        }

        public void PopRuleSet() {
            _ruleSets.Pop();
        }

    }
}
