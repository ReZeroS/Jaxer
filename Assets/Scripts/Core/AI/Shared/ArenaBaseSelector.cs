using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Config;
using Random = UnityEngine.Random;

namespace ReZeros.Jaxer.Core.AI.Shared
{
    public class ArenaBaseSelector : Composite
    {

        public string TaskCorner;
        public string TaskCenter;
        public string TaskCloseup;
        
        
        [UnityEngine.Tooltip("Seed the random number generator to make things easier to debug")]
        public int seed = 0;
        [UnityEngine.Tooltip("Do we want to use the seed?")]
        public bool useSeed = false;

        // A list of indexes of every child task. This list is used by the Fischer-Yates shuffle algorithm.
        private List<int> childIndexList = new();
        // The random child index execution order.
        private Stack<int> childrenExecutionOrder = new();
        // The task status of the last child ran.
        private TaskStatus executionStatus = TaskStatus.Inactive;

        BossConfig bossConfig;
        
        enum ArenaLocation
        {
            Corner, Center, Closeup
        }
        
        
        public override void OnAwake()
        {
            // If specified, use the seed provided.
            if (useSeed) {
                Random.InitState(seed);
            }
            bossConfig = GetComponent<BossConfig>();
        }

        public override void OnStart()
        {
            var arenaLocation = GetRelativeArenaLocation();
            var tasks = arenaLocation switch
            {
                ArenaLocation.Corner => TaskCorner,
                ArenaLocation.Center => TaskCenter,
                ArenaLocation.Closeup => TaskCloseup,
                _ => throw new ArgumentOutOfRangeException()
            };
            // Select considered child indices based on the current stage
            childIndexList.Clear();
            childIndexList = tasks.Split(',').Select(int.Parse).ToList();
            
            // Randomize the indecies
            ShuffleChilden();
        }

        private ArenaLocation GetRelativeArenaLocation()
        {
            var relativeArenaPosX = transform.position.x - bossConfig.arenaCenter.position.x;
            var facing = transform.localScale.x;
            var normalizedX = relativeArenaPosX / bossConfig.arenaRadius * facing;
            if (normalizedX < -0.33f)
            {
                return ArenaLocation.Corner;
            } 
            if (normalizedX < 0.33f)
            {
                return ArenaLocation.Center;
            }
            return ArenaLocation.Closeup;
        }
        
        
        public override int CurrentChildIndex()
        {
            // Peek will return the index at the top of the stack.
            return childrenExecutionOrder.Peek();
        }

        public override bool CanExecute()
        {
            // Continue exectuion if no task has return success and indexes still exist on the stack.
            return childrenExecutionOrder.Count > 0 && executionStatus != TaskStatus.Success;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Pop the top index from the stack and set the execution status.
            if (childrenExecutionOrder.Count > 0) {
                childrenExecutionOrder.Pop();
            }
            executionStatus = childStatus;
        }

        public override void OnConditionalAbort(int childIndex)
        {
            // Start from the beginning on an abort
            childrenExecutionOrder.Clear();
            executionStatus = TaskStatus.Inactive;
            ShuffleChilden();
        }

        public override void OnEnd()
        {
            // All of the children have run. Reset the variables back to their starting values.
            executionStatus = TaskStatus.Inactive;
            childrenExecutionOrder.Clear();
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            seed = 0;
            useSeed = false;
        }

        private void ShuffleChilden()
        {
            // Use Fischer-Yates shuffle to randomize the child index order.
            for (int i = childIndexList.Count; i > 0; --i) {
                int j = Random.Range(0, i);
                int index = childIndexList[j];
                childrenExecutionOrder.Push(index);
                childIndexList[j] = childIndexList[i - 1];
                childIndexList[i - 1] = index;
            }
        }
        
    }
}
