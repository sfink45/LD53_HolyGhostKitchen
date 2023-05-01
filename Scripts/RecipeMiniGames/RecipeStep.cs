using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RecipeMiniGames
{
    public abstract class RecipeStep : MonoBehaviour
    {
        public event StepCompletedHandler OnStepCompleted;
        public delegate void StepCompletedHandler(float score);
        [SerializeField] public Image _icon;
        [SerializeField] protected float _targetValue;
        [SerializeField] protected float _perfectScoreTolerance;
        [SerializeField] protected float _goodScoreTolerance;
        [SerializeField] protected float _okayScoreTolerance;
        protected List<StepRank.Ranks> _ranks;

        public virtual bool IsInteractiveStep { get; protected set; }
        public bool IsActivated;

        public virtual void HandleActiveUpdate(float deltaTime) { }
        public virtual void HandlePassiveUpdate(float deltaTime, Image Icon) { }

        private void Awake()
        {
        }

        public virtual void Activate()
        {
            IsActivated = true;
            _ranks = new List<StepRank.Ranks>();
            if(IsInteractiveStep) transform.DOMove(Vector3.zero, .5f).SetEase(Ease.InOutCirc);
        }

        protected void StepCompleted()
        {
            var score = StepRank.GetScore(_ranks);
            OnStepCompleted?.Invoke(score);
        }

        protected (StepRank.Ranks rank, string feedback) CalculateScore(float toScoreValue, bool allowSmallerValues = false, string overFeedback = "Too Big", string underFeedback = "Too Small")
        {
            var result = StepRank.Ranks.Perfect;
            if (_perfectScoreTolerance + _targetValue >= toScoreValue && toScoreValue >= _targetValue - _perfectScoreTolerance)
            {
                Debug.Log("PERFECT CHOP!");
                result = (StepRank.Ranks.Perfect);
            }
            else if (_goodScoreTolerance + _targetValue >= toScoreValue && toScoreValue >= _targetValue - _goodScoreTolerance ||
                    (_goodScoreTolerance + _targetValue >= toScoreValue && allowSmallerValues))
            {
                Debug.Log("GOOD CHOP");
                result = (StepRank.Ranks.Good);
            }
            else if (_okayScoreTolerance + _targetValue >= toScoreValue && toScoreValue >= _targetValue - _okayScoreTolerance)
            {
                Debug.Log("okay chop");
                result = (StepRank.Ranks.Okay);
            }
            else
            {
                Debug.Log("Poor Chop");
                result = (StepRank.Ranks.Poor);
            }
            var feedback = "";
            if (result != StepRank.Ranks.Perfect)
                feedback = _perfectScoreTolerance + _targetValue < toScoreValue ? overFeedback : underFeedback;
            _ranks.Add(result);
            return (result, feedback);
        }

        public abstract void Destroy();
    }
}
