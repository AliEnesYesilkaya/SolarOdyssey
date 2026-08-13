namespace SolarOdyssey.Enemy
{
    public interface IState //sınıfların kalıtım alacağı ana arayüz
    { 
        void Enter();
        void Tick();
        void Exit(); 
    }
}