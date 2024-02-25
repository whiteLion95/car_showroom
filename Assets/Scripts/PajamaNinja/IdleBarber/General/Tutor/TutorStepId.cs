namespace PajamaNinja.CarShowRoom
{
    public enum TutorStepId
    {
        None = 0,
        // Wheel parts
        WheelParts_Take,
        WheelParts_Unload,
        // Gear parts
        GearParts_Take,
        GearParts_Unload,
        // Sheet parts
        SheetParts_Take,
        SheetParts_Unload,
        // Go and park car
        ParkCar_GoToCar,
        ParkCar_Park,
        // Sell car
        SellCar,
        // Get car money
        GetCarMoney,
        // Buy car design
        BuyCarDesign_AdminDesk,
        BuyCarDesign_Buy,
        //Equip car desing
        EquipCarDesing_AdminDesk,
        EquipCarDesing_Equip,
        EquipCarDesing_Close,
        // Open conveyor2 tab
        OpenConveyor2Tab_AdminDesk,
        OpenConveyor2Tab_Open
    }
}