using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace BoxesLibrary
{
    public class Manager
    {
        private BST<Width> _mainTree;
        private Linked_List<TimeData> _timeList;
        private ICommunicate _printBox;
        private ConfigurationData _configData;
        private Timer _timer;

        public Manager(ICommunicate myBox, ConfigurationData configurationData)
        {
            _mainTree = new BST<Width>();
            _timeList = new Linked_List<TimeData>();
            _printBox = myBox;
            _configData = configurationData;

            DateTime tomorrow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            TimeSpan due = tomorrow - DateTime.Now;
            TimeSpan period = new TimeSpan(_configData.FrequencyCheck, 0, 0, 0);
            _timer = new Timer(RemovingEventHandler, null, due, period); //Initiazling the timer
        }

        public bool AddSupply(double width, double height, int count) //Checking if we just have to update count or create new box size
        {
            if (width <= 0 || height <= 0 || count <= 0) throw new ArgumentException("One of the inputs is invalid");
            width = Math.Round(width, _configData.DigitsSensitivity);
            height = Math.Round(height, _configData.DigitsSensitivity);

            Height heightData = new Height(height, count);
            Width foundWidth, widthData = new Width(width, heightData);

            if (_mainTree.SearchAndInsert(widthData, out foundWidth)) //If we found the width in the tree
            {
                Height foundHeight;

                if (foundWidth.HeightTree.SearchAndInsert(heightData, out foundHeight)) //If we found the height in the tree
                {
                    CheckMaxAmount(foundHeight.Count + count); //Alerting the user if we exceed the max amount of boxes, and how many boxes returned back
                    foundHeight.Count = Math.Min(foundHeight.Count + count, _configData.MaxQuantity); //If we did, just update count
                    return true;
                }
                else //Width exists, the height is new
                {
                    _timeList.AddLast(new TimeData(width, height)); //Adding the new data to the dates list
                    heightData.timeListNodeRef = _timeList.End; //Assigning the new timeList node to timeListNodeRef node of height class
                }
            }
            else //Width and Height are new
            {
                _timeList.AddLast(new TimeData(width, height)); //Adding the new data to the dates list
                heightData.timeListNodeRef = _timeList.End; //Assigning the new timeList node to timeListNodeRef node of height class
            }

            CheckMaxAmount(count); //Checking if we passed the max amount of boxes count and alert the user if we did
            return false;
        }

        public bool GetDetails(double width, double height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("One of the inputs is invalid");
            width = Math.Round(width, _configData.DigitsSensitivity);
            height = Math.Round(height, _configData.DigitsSensitivity);

            Width foundWidth;
            Height foundHeight;

            if (_mainTree.Search(new Width(width), out foundWidth)) //Searching for the specific width value
            {
                if (foundWidth.HeightTree.Search(new Height(height), out foundHeight)) //Searching for the specific height value
                { //If we found the box with the input values - print its details
                    _printBox.PrintFullBoxData(foundWidth.WidthValue, foundHeight.HeightValue, foundHeight.Count, foundHeight.timeListNodeRef.data.DateLastBought.ToString("dd/MM/yyyy"));
                    return true;
                }
            }

            return false;
        }

        public bool FindBestMatch(double width, double height, int count)
        {
            if (width <= 0 || height <= 0 || count <= 0) throw new ArgumentException("One of the inputs is invalid");
            width = Math.Round(width, _configData.DigitsSensitivity);
            height = Math.Round(height, _configData.DigitsSensitivity);

            return SearchBestBoxForGift(width, height, count); //Searching the best box match for the gift
        }   

        private bool SearchBestBoxForGift(double width, double height, int countInputUser)
        {
            int sumCountBoxes = 0, currentSplits = 0;
            string strTemp = string.Empty;
            Width foundWidth = default, widthToSearch = new Width(width);
            Height foundHeight = default, heightToSearch = new Height(height);
            Linked_List<BoxCategory> boxCategories = new Linked_List<BoxCategory>();

            while (_mainTree.SearchTheClosest(widthToSearch, out foundWidth) && CheckBoxValue(width, foundWidth.WidthValue) && currentSplits < _configData.MaxSplits) //Searching the closest width and checking it doesn't exceed the limits
            {
                while (foundWidth.HeightTree.SearchTheClosest(heightToSearch, out foundHeight) && CheckBoxValue(height, foundHeight.HeightValue) && currentSplits < _configData.MaxSplits) //Searching the closest height and checking it doesn't exceed the limits
                {
                    sumCountBoxes += foundHeight.Count;
                    
                    if (sumCountBoxes - countInputUser >= 0)//Checking if count of all boxes in the split list in addition to the current box count meet the requirements of count input
                        {
                        if (currentSplits > 0 && !ManageFoundBoxes(ref countInputUser, sumCountBoxes, currentSplits, foundWidth, foundHeight, boxCategories)) return false;

                        foundHeight.Count -= countInputUser; //Updating the height's count

                        if (foundHeight.Count == 0) //Checking if it was the last box in the stock
                        {
                            RemoveByQuantity(foundWidth, foundHeight);
                            _printBox.PrintBoxOptionDeleted(foundWidth.WidthValue, foundHeight.HeightValue); //Printing the last box with deleted label
                            _printBox.SuccessMessage(); //Printing Success message
                            return true;
                        }

                        _timeList.MoveToEnd(foundHeight.timeListNodeRef); //Moving the latest box node to the end of the list
                        _timeList.End.data.DateLastBought = DateTime.Now; //Updating the date of the latest box node
                        //Checking if the box size didn't go below the minimum boxes limit, if it did - print alert to user in order to restock the box size
                        if (CheckMinAmount(foundHeight.Count)) _printBox.AlertMinQuantity(foundWidth.WidthValue, foundHeight.HeightValue, foundHeight.Count);

                        if (currentSplits == 0) _printBox.PrintBoxData(foundWidth.WidthValue, foundHeight.HeightValue, countInputUser); //Printing the found box size
                        _printBox.SuccessMessage(); //Printing success message and return true
                        return true;
                    }
                    
                    else //If we do - add to split list and keep searching for the next box that may be suitable for the user needs
                    {
                        boxCategories.AddLast(new BoxCategory(foundWidth, foundHeight));
                        currentSplits++;
                    }
                    heightToSearch.HeightValue = foundHeight.HeightValue + _configData.DeltaValue; //Adding delta value to height value to continue searching for suitable box size
                }
                widthToSearch.WidthValue = foundWidth.WidthValue + _configData.DeltaValue; //Adding delta value to width value to continue searching for suitable box size
            }
            _printBox.FailureMessage(); //If we didn't find a suitable box, print failure message and return false
            return false;
        }

        private bool ManageFoundBoxes(ref int countInputUser, int sumCountBoxes, int currentSplits, Width tempWidth, Height tempHeight, Linked_List<BoxCategory> boxCategories)
        {
            if (_printBox.GetUserAnswer(currentSplits + 1)) //Asking the user if he wants to watch the split options (+ 1 is for the last one)
            {
                PrintBoxCategories(tempWidth.WidthValue, tempHeight.HeightValue, countInputUser - (sumCountBoxes - tempHeight.Count), boxCategories); //Printing the split options
                if (_printBox.DoesUserAgree()) //Asking the user if there is a permission to submit the split
                {
                    countInputUser -= sumCountBoxes - tempHeight.Count; //Updating the input count of the user
                    Split(boxCategories); //Submit the splitting (removing the nodes from the main tree and dates list, and printing them)
                }
                else
                {
                    _printBox.FailureMessage(); //If the user doesn't agree splitting - printing failure message and return false
                    return false;
                }
            }
            else
            {
                _printBox.FailureMessage(); //If the user doesn't want to watch splitting options - printing failure message and return false
                return false;
            }
            return true;
        }

        private void RemovingEventHandler(object param)
        { //Looping over the time list and check if the box size is considered as old and need to be deleted
            while (_timeList.Start != null && DateTime.Now > _timeList.Start.data.DateLastBought.AddDays(_configData.LifeTime))
            {
                RemoveByTime(_timeList.Start.data.WidthValue, _timeList.Start.data.HeightValue);
            }
        }

        private void RemoveByTime(double width, double height)
        {
            Width widthToSearch = new Width(width), foundWidth;
            Height inputHeight = new Height(height);

            _timeList.RemoveFirst(out _timeList.Start.data); //Removing the first node in time list
            _mainTree.Search(widthToSearch, out foundWidth); //Getting the width reference of the width value we want to delete

            foundWidth.HeightTree.Remove(inputHeight); //Removing the input height node of the the width height tree
            if (foundWidth.HeightTree.IsEmpty()) _mainTree.Remove(foundWidth); //Checking if the height value we deleted was the last one in the height tree, if it was - delete the found width
        }

        private void RemoveByQuantity(Width foundWidth, Height foundHeight)
        {
            foundWidth.HeightTree.Remove(foundHeight); //Deleting the specific node of the box size
            if (foundWidth.HeightTree.IsEmpty()) _mainTree.Remove(foundWidth); //Checking if there are any height sizes in the width tree, if not - remove the width from the main tree
            _timeList.RemoveByNode(foundHeight.timeListNodeRef); //Deleting the node in the dates list
        }

        private bool CheckMinAmount(int amountNow) //Checking if the input amount is smaller or equal to the minimum amount limit
        {
            return amountNow <= _configData.MinQuantity;
        }

        private void CheckMaxAmount(int sumCount)
        {
            if (sumCount > _configData.MaxQuantity) //Checking if the sum of the current count and input count is higher from the max quantity, if it is - alert the user
            {
                _printBox.AlertMaxQuantity(_configData.MaxQuantity, sumCount - _configData.MaxQuantity);
            }
        }

        private void Split(Linked_List<BoxCategory> boxCategories) //Executing the split operation with alerts for the user
        {
            foreach (var box in boxCategories)
            {
                _printBox.PrintBoxOptionDeleted(box.WidthValue.WidthValue, box.HeightValue.HeightValue); //Alerting the user that the box was deleted and out of stock
                RemoveByQuantity(box.WidthValue, box.HeightValue);
            }
        }

        private void PrintBoxCategories(double width, double height, int count, Linked_List<BoxCategory> boxCategories)
        {//Printing the all the boxes data in the split list and the current box
            foreach (var box in boxCategories)
            {
                _printBox.PrintBoxData(box.WidthValue.WidthValue, box.HeightValue.HeightValue, box.HeightValue.Count);
            }

            _printBox.PrintBoxData(width, height, count);
        }

        private bool CheckBoxValue(double startValue, double foundValue) //Checking if the box width or height doesn't exceed the configuration limits
        {
            double upperBound = startValue * _configData.CoefficientValue;
            if (foundValue <= upperBound)
                return true;

            return false;
        }
    }
}
