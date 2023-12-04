RegisterCommand("create", function (sender, args)
    if not IsValidObject(args[1]) then
        Print("Not a Valid Object");
        return;
    end
    obj_guid = LoadObject(args[1])
    while not ObjectExists(obj_guid) do
        Wait(10)
    end
    ref_id = CreateObject(obj_guid, tonumber(args[2]), tonumber(args[3]), tonumber(args[4]))
    Print(ref_id)
    MoveObject(ref_id,1,1,1);
end)
RegisterCommand("move", function (_,args)
    MoveObject(args[1],tonumber(args[2]),tonumber(args[3]),tonumber(args[4]))
end)

