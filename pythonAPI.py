import mlagents
import numpy as np
from mlagents_envs.environment import UnityEnvironment as UE
env = UE(file_name='Roll-a-ball/Roll-a-ball', seed=1, side_channels=[])
env.reset()
# Get behaviour name

behavior_name = list(env.get_behavior_names())[0]
print(behavior_name)

# get behavior spec of the certain  ehavior
spec = env.get_behavior_spec(behavior_name)
print(spec)
print("No. of observations: {}".format(spec.observation_shapes))

# Check the type of action of the agent
if spec.is_action_continuous():
    print("The action is continuous")
elif spec.is_action_discrete():
    print("The action is discrete")

env.reset()
decision_steps, terminal_steps = env.get_steps(behavior_name)
print(list(decision_steps))

for episode in range(3):
    tracked_agent = -1
    done = False
    episode_rewards = 0
    while not done:
        if tracked_agent == -1 and len(decision_steps) ==1:
            tracked_agent = decision_steps.agent_id[0]

        # Generate an action for all agents
        num_agents = len(decision_steps)
        action = np.random.randint(2, size=(num_agents, spec.action_size))
        
        # Set the actions
        env.set_actions(behavior_name, action)
        # Move the simulation forward
        env.step()
        # Get the new simulation results
        decision_steps, terminal_steps = env.get_steps(behavior_name) # these are obs,reward, agent_id, action_mask 
        print(terminal_steps)
        if tracked_agent in decision_steps:
            episode_rewards += decision_steps[tracked_agent].reward
        if tracked_agent in terminal_steps:
            episode_rewards += terminal_steps[tracked_agent].reward
            done = True
            print("done")
    print("Total rewards for episode {} is {}".format(episode, episode_rewards))

