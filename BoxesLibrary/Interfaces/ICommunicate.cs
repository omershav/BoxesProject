using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public interface ICommunicate
    {
        bool GetUserAnswer(int count);

        bool DoesUserAgree();

        void PrintBoxData(double width, double height, int count);

        void PrintFullBoxData(double width, double height, int count, string date);

        void PrintBoxOptionDeleted(double width, double height);

        void AlertMinQuantity(double width, double height, int count);

        void AlertMaxQuantity(int quantityNow, int countNotUsed);

        void SuccessMessage();

        void FailureMessage();
    }
}
