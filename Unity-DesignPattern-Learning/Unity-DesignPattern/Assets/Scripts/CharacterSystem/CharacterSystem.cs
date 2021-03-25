using System;
using System.Collections.Generic;

public class CharacterSystem : IGameSystem
{
    private List<ICharacter> mEnemys = new List<ICharacter>();
    private List<ICharacter> mSoldiers = new List<ICharacter>();

    public void AddEnemy(IEnemy enemy)
    {
        mEnemys.Add(enemy);
    }
    public void RemoveEnemy(IEnemy enemy)
    {
        mEnemys.Remove(enemy);
    }
    public void AddSoldier(ISoldier soldier)
    {
        mSoldiers.Add(soldier);
    }
    public void RemoveSoldier(ISoldier soldier)
    {
        mSoldiers.Remove(soldier);
    }

    public override void Update()
    {
        UpdateEnemys();
        UpdateSoldiers();

        RemoveCharacterIsKilled(mEnemys);
        RemoveCharacterIsKilled(mSoldiers);
    }

    private void UpdateEnemys()
    {
        foreach (var e in mEnemys)
        {
            e.Update();
            e.UpdateFSMAI(mSoldiers);
        }
    }
    private void UpdateSoldiers()
    {
        foreach (var s in mSoldiers)
        {
            s.Update();
            s.UpdateFSMAI(mEnemys);
        }
    }

    private void RemoveCharacterIsKilled(List<ICharacter> characters)
    {
        List<ICharacter> canDestroys = new List<ICharacter>();
        foreach (var character in characters)
        {
            if (character.CanDestroy)
            {
                canDestroys.Add(character);
            }
        }

        foreach (var character in canDestroys)
        {
            character.Release();
            characters.Remove(character);
        }
    }

    public void RunVisitor(ICharacterVisitor visitor)
    {
        foreach (var character in mEnemys)
        {
            character.RunVisitor(visitor);
        }
        foreach (var character in mSoldiers)
        {
            character.RunVisitor(visitor);
        }
    }
}
