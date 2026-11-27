using System;

static class Program
{
    static void Require(bool value,string description)
    {
        if(!value)throw new Exception(description);
    }
    static void Reject(Action action,string description)
    {
        try{action();}catch(ArgumentOutOfRangeException){return;}
        throw new Exception(description);
    }
    static void Main()
    {
        const string order="ABBAABBAAB";
        var settings=new RiftorynGameSettings();
        Require(settings.TeamSize==5&&settings.Opponent&&!settings.ManualDefense&&!settings.LegacyLan,"Default settings changed.");
        for(int size=1;size<=5;size++)
        {
            settings.TeamSize=size;
            foreach(bool opponent in new[]{false,true})
            {
                settings.Opponent=opponent;
                int teamA=0,teamB=0;
                Require(settings.PickCount==size*(opponent?2:1),"Wrong number of draft turns.");
                for(int turn=0;turn<settings.PickCount;turn++)
                {
                    bool isA=settings.IsTeamTurn(turn);
                    Require(isA==(!opponent||order[turn]=='A'),"Draft order changed.");
                    if(isA)teamA++;else teamB++;
                }
                Require(teamA==size&&teamB==(opponent?size:0),"Draft gives the wrong number of heroes to a team.");
                Reject(()=>settings.IsTeamTurn(-1),"Negative draft index was accepted.");
                Reject(()=>settings.IsTeamTurn(settings.PickCount),"Draft continued after its final pick.");
            }
        }
        Reject(()=>settings.TeamSize=0,"Zero heroes was accepted.");
        Reject(()=>settings.TeamSize=6,"More than five heroes was accepted.");
        Require(settings.TowerLimit==5,"Default tower limit changed.");
        for(int count=1;count<=7;count++){settings.TowerLimit=count;Require(settings.TowerLimit==count,"Tower limit was not retained.");}
        Reject(()=>settings.TowerLimit=0,"Zero towers was accepted.");
        Reject(()=>settings.TowerLimit=8,"More than seven towers was accepted.");
        Console.WriteLine("OK: 10 configurations, team sizes 1–5, tower limits 1–7, draft order and invalid inputs.");
    }
}
