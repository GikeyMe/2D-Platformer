using UnityEngine;
using System.Collections;

public interface IAgentState {

    void Act();
    void Activate(Agent agent);
}
