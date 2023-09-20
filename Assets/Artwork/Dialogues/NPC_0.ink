VAR QuestFirstSteps = false
-> FirstTalk

===FirstTalk===
Welcome new friend... #speaker: Lauren #portrait:npc_0 #layout:right
Looking for a job?
 * [Yeah.]
    That's great!
    ~QuestFirstSteps = true
 * [Rather not.]
    Why not? Don't want some coins?
    * * [...]
        I thought so.
    ~QuestFirstSteps = true

 - There is a bandit prowling <b><color=\#C51E36>on the north</color></b>. Please take care of him. 
-> END