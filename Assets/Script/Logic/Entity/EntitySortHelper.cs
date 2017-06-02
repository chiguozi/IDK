using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySortHelper
{
    EntityBase _target;
    List<List<string>> _args;
    int SortHp(EntitySprite e1, EntitySprite e2)
    {
        int res = 0;
        if (e1 == null && e2 != null)
            res = -1;
        else if (e1 != null && e2 == null)
            res = 1;
        else
        {
            res = e1.HP.CompareTo(e2.HP);
        }
        return res;
    }

    int SortDistance(EntityBase e1, EntityBase e2)
    {
        var sqrDis1 = ( e1.position - _target.position ).XZMagnitude();
        var sqrDis2 = ( e2.position - _target.position ).XZMagnitude();
        int res = sqrDis1.CompareTo(sqrDis2);
        return res;
    }

    int SortEntityType(EntityBase e1, EntityBase e2, List<EntityType> typeList)
    {
        int index1 = typeList.IndexOf(e1.entityType);
        int index2 = typeList.IndexOf(e2.entityType);
        return -index1.CompareTo(index2);
    }

    int SortEntityCamp(EntityBase e1, EntityBase e2, List<CampType> campList)
    {
        int index1 = campList.IndexOf(Util.GetTargetCampType(_target, e1));
        int index2 = campList.IndexOf(Util.GetTargetCampType(_target, e2));
        return -index1.CompareTo(index2);
    }


    public void Init(EntityBase target, List<List<string>> args)
    {
        _target = target;
        _args = args;
    }

    public void SortList(List<EntityBase> sortList)
    {
        sortList.Sort(SortFunc);
    }


    int SortFunc(EntityBase e1, EntityBase e2)
    {
        for(int i = 0; i < _args.Count; i++)
        {
            int value = SortByParams(e1, e2, _args[i]);
            if (value != 0)
                return value;
        }
        return 0;
    }

    int SortByParams(EntityBase e1, EntityBase e2, List<string> args)
    {
        int type = StringUtil.ParseIntFromList(args, 0, 0);
        int reverse = 1;
        switch (type)
        {
            //HP
            case 1:
                reverse = StringUtil.ParseIntFromList(args, 1, 0) == 1 ? -1 : 1;
                return reverse * SortHp(e1 as EntitySprite, e2 as EntitySprite);
            case 2:
                reverse = StringUtil.ParseIntFromList(args, 1, 0) == 1 ? -1 : 1;
                return reverse * SortDistance(e1, e2);
            case 3:
                List<EntityType> typeList = new List<EntityType>();
                for(int i = 1; i < args.Count; i++)
                {
                    typeList.Add((EntityType)StringUtil.ParseIntFromList(args, i, 0));
                }
                return SortEntityType(e1, e2, typeList);
            case 4:
                List<CampType> campList = new List<CampType>();
                for (int i = 1; i < args.Count; i++)
                {
                    campList.Add((CampType)StringUtil.ParseIntFromList(args, i, 0));
                }
                return SortEntityCamp(e1, e2, campList);
        }
        return 0;
    }


}
