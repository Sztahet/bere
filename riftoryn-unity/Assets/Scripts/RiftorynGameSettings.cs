using System;

/// <summary>Parameters for the single local game flow. LAN is a separately labelled legacy test.</summary>
public sealed class RiftorynGameSettings
{
    public const int DefaultTowerLimit=5;
    static readonly bool[] TeamOrder={true,false,false,true,true,false,false,true,true,false};
    int teamSize=5, towerLimit=DefaultTowerLimit;

    public int TeamSize
    {
        get=>teamSize;
        set
        {
            if(value<1||value>5)throw new ArgumentOutOfRangeException(nameof(value),"Team size must be between 1 and 5.");
            teamSize=value;
        }
    }
    public bool Opponent { get; set; }=true;
    public bool ManualDefense { get; set; }
    public bool LegacyLan { get; set; }
    public int PickCount=>TeamSize*(Opponent?2:1);
    public int TowerLimit
    {
        get=>towerLimit;
        set
        {
            if(value<1||value>7)throw new ArgumentOutOfRangeException(nameof(value),"Tower limit must be between 1 and 7.");
            towerLimit=value;
        }
    }

    public bool IsTeamTurn(int index)
    {
        if(index<0||index>=PickCount)throw new ArgumentOutOfRangeException(nameof(index));
        return !Opponent||TeamOrder[index];
    }
}
