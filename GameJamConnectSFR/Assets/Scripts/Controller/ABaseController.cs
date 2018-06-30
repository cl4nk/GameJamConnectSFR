using System.Collections.Generic;
using Assets.Scripts.Tower;
using Project.Scripts.StateMachineBehaviour;
using UnityEngine;

namespace Project.Scripts.Controller
{
	public abstract class ABaseController : MonoBehaviour
	{
	    public static List<ABaseController> s_controllerList = new List<ABaseController>();
	    protected static List<MonoBehaviour> s_lockingBehaviours = new List<MonoBehaviour>();

	    public static void RegisterForLock(MonoBehaviour monoBehaviour)
	    {
	        if (s_lockingBehaviours.Contains(monoBehaviour) == false)
	        {
	            s_lockingBehaviours.Add(monoBehaviour);
	        }

	        EnableActionsOnAllControllers(s_lockingBehaviours.Count == 0);
	    }

	    public static void UnregisterForLock(MonoBehaviour monoBehaviour)
	    {
	        s_lockingBehaviours.Remove(monoBehaviour);
	        EnableActionsOnAllControllers(s_lockingBehaviours.Count == 0);
	    }

	    protected static void EnableActionsOnAllControllers(bool enabled)
	    {
	        foreach (ABaseController controller in s_controllerList)
	        {
	            controller.EnableControl = enabled;
	            if (enabled == false && controller.PossessedPawn)
	            {
	            //    controller.PossessedPawn.Reset();
                }
	        }
	    }

        public Tower DefaultPawn;
		public Tower PossessedPawn { get; private set; }

	    private bool m_enableControl = true;

	    public bool EnableControl
	    {
	        get { return m_enableControl; }
	        set
	        {
	            if (m_enableControl != value)
	            {
	                m_enableControl = value;
	                if (m_enableControl)
	                {
	                    OnEnableControl();
	                }
	                else
	                {
	                    OnDisableControl();
	                }
	            }
	        }
	    }

	    public bool AttachOnEnable = true;

	    protected virtual void Awake()
	    {
	        s_controllerList.Add(this);

	        if (s_lockingBehaviours.Count > 0)
	        {
	            EnableControl = false;
	        }
        }

	    protected virtual void OnDestroy()
	    {
	        s_controllerList.Remove(this);
	    }

	    public virtual void OnEnable()
		{
		    if (AttachOnEnable && PossessedPawn == null)
		    {
          //      AttachTo(DefaultPawn);
		    }
        }

		public virtual void OnDisable()
		{
		    Detach();
		}

	    protected virtual void OnEnableControl()
	    {

	    }

	    protected virtual void OnDisableControl()
	    {

	    }

        protected virtual void ControllerUpdate()
        { }

	    private void Update()
	    {
            //TODO: Remove that: launched when speaker try to speak when dead
            int i = 0;
            bool changed = false;
            while (i < s_lockingBehaviours.Count)
            {
                if (s_lockingBehaviours[i] == null)
                {
                    s_lockingBehaviours.RemoveAt(i);
                    changed = true;
                    continue;
                }
                ++i;
            }
            if (changed)
            {
                EnableActionsOnAllControllers(s_lockingBehaviours.Count == 0);
            }

	        if (PossessedPawn && EnableControl && Time.timeScale > 0)
	        {
	            ControllerUpdate();
	        }
	    }
/*
		public void AttachTo( Pawn pawn )
		{
			if ( pawn && pawn != PossessedPawn )
			{
				Detach();

				PossessedPawn = pawn;
				PossessedPawn.PossessingController = this;
                SceneLinkedSMB<ABaseController>.Initialise(PossessedPawn.Animator, this);
				Possess( PossessedPawn );
			}
		}
        
		protected virtual void Possess( Pawn pawn )
		{
		}
        */
		public void Detach()
		{
			if ( PossessedPawn )
			{
				Unpossess();

             /*   if ( PossessedPawn.PossessingController == this )
				{
					PossessedPawn.PossessingController = null;
				}*/

				PossessedPawn = null;
			}
		}

		protected virtual void Unpossess()
		{
		}

	    public void PawnJump()
	    {
	        if (PossessedPawn)
	        {
	         //   PossessedPawn.Jump();
	        }
	    }

        public virtual void TryVibrate(float duration, float leftPower, float rightPower)
        { }
	}
}