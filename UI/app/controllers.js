'use strict';

var tempUserId = 0;
var tempUserLogin = "";

var tempCategory = "";
var tempSubcategory = "";
var tempSearchField = "";

var isManager = false;
var isAdmin = false;

var app = angular.module('myApp');
app.controller('signIn', function ($scope, $http, $route) {
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    if (tempUserLogin != "") $scope.logged = true;

    $scope.check = function () {
        $scope.manager = isManager;
        $scope.admin = isAdmin;

        $http.get("http://localhost:58135/api/user/" + $scope.login + "/" + $scope.password).then(
            function (response) {
                tempUserLogin = response.data.Login;
                tempUserId = response.data.UserId;
                if (response.data.Status == "User") {
                    isManager = false; isAdmin = false;
                }
                if (response.data.Status == "Manager") {
                    isManager = true; isAdmin = false;
                }
                if (response.data.Status == "Admin") {
                    isManager = true; isAdmin = true;
                }
                $scope.login = "";
                $scope.password = "";
                $route.reload(); 

            },
            function (response) {
                $scope.showWarningMessage = true;
                if (!response.data.Message.includes('No HTTP resource was found') &&
                    !response.data.Message.includes('The request is invalid')) $scope.warningMessage = response.data.Message;
                else $scope.warningMessage = "Correct your entered data!"
            }
        );
    };
    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        $route.reload();
    };
});

app.controller('register', function ($scope, $http, $route) {
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    if (tempUserLogin != "") $scope.logged = true;
    $scope.registerin = function () {
        var data = {
            "Name": $scope.name,
            "Surname": $scope.surname,
            "Patronymic": $scope.patronymic,
            "Login": $scope.login,
            "Password": $scope.password,
            "PhoneNumber": $scope.phone,
            "Passport": $scope.passport,
            "Status": "User"
        };
        $http.post('http://localhost:58135/api/user/newUser', data)
            .then(
            function (response) {
                alert("Registered");
                $scope.name = "";
                $scope.surname = "";
                $scope.patronymic = "";
                $scope.phone = "";
                $scope.login = "";
                $scope.password = "";
                $scope.passport = "";
            },
            function (response) {
                $scope.showWarningMessage = true;
                if (!response.data.Message.includes('No HTTP resource was found') &&
                    !response.data.Message.includes('The request is invalid')) $scope.warningMessage = response.data.Message;
                else $scope.warningMessage = "Correct your entered data!"
            });
    };
    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        $route.reload();
    };
});

var name = "";
var specification = "";
var bet = "";
var duration = "";
var step = "";
app.controller('lotCreate', function ($scope, $http, $route) {
    $scope.name = name;
    $scope.specification = specification;
    $scope.bet = bet;
    $scope.duration = duration;
    $scope.step = step;

    $http.get("http://localhost:58135/api/category").then(function (response) {
        $scope.categories = response.data;
    });
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    if (tempUserLogin != "") $scope.logged = true;
    var finded = false; 
    $scope.lookSubc = function () {
        if (finded == true) {
            name = $scope.name;
            specification = $scope.specification;
            bet = $scope.bet;
            duration = $scope.duration;
            step = $scope.step;
            $route.reload();
        }
    }
    $scope.findSubcstegories = function () {
        $http.get("http://localhost:58135/api/subcategory/get/" + $scope.selectedCategory.trim()).then(function (response) {
            $scope.subcategories = response.data
            finded = true;
        });
    }

    $scope.create = function () {
        var selectedCatg = $scope.selectedCategory;
        var selectedSubc = $scope.selectedSubcategory;
        if (selectedCatg != undefined) selectedCatg = selectedCatg.trim();
        if (selectedSubc != undefined) selectedSubc = selectedSubc.trim();
        var data = {
            "Name": $scope.name,
            "Specification": $scope.specification,
            "Bet": $scope.bet,
            "Category": selectedCatg,
            "Subcategory": selectedSubc,
            "Duration": $scope.duration,
            "Owner": tempUserId,
            "Step": $scope.step
        };
        $http.post('http://localhost:58135/api/lot/newLot', data)
            .then(
            function (response) {
                alert("Added");
                $scope.name = "";
                $scope.specification = "";
                $scope.bet = "";
                $scope.category = "";
                $scope.subcategory = "";
                $scope.duration = "";
                $scope.owner = "";
                $scope.step = "";
            },
            function (response) {
                $scope.showWarningMessage = true;
                if (!response.data.Message.includes('No HTTP resource was found') &&
                    !response.data.Message.includes('The request is invalid')) $scope.warningMessage = response.data.Message;
                else $scope.warningMessage = "Correct your entered data!"
            });
    };
    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        window.location.href = "http://localhost:58352/index.html#!/lotsView";
    };
});

app.controller('lotsView', function ($scope, $http, $route) {
    $scope.selectedCategory = tempCategory;
    $scope.selectedSubcategory = tempSubcategory;
    $scope.searchField = tempSearchField;
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    
    $scope.owner = false;
    var tempLotId = 0;
    if (tempUserLogin != "") $scope.logged = true;

    $http.get("http://localhost:58135/api/category").then(function (response) {
        $scope.categories = response.data;
    });
    
    if (tempCategory != "" || tempSubcategory != "" || tempSearchField != "") {
        $http.get("http://localhost:58135/api/lot/GetLotsBySearch?category=" + tempCategory +
                "&subcategory=" + tempSubcategory +
                "&keyword=" + tempSearchField)
            .then(function (response) {
                $scope.lots = response.data;
            });
    }
    else {
        $http.get("http://localhost:58135/api/lot/GetConfirmedLots")
            .then(function (response) {
                $scope.lots = response.data;
            });
    }

    $scope.editLot = function () {
        $http.put("http://localhost:58135/api/lot/change/" + $scope.name + "/" + $scope.specification + "/" + tempLotId)
            .then(
            function (response) {
                alert("Sucsses");
                $route.reload();
            },
            function (response) {
                $scope.showWarningMessage = true;
                if (!response.data.Message.includes('No HTTP resource was found') &&
                    !response.data.Message.includes('The request is invalid')) $scope.warningMessage = response.data.Message;
                else $scope.warningMessage = "Correct your entered data!"
            });
    };

    $scope.deleteLot = function () {
        $http.delete("http://localhost:58135/api/lot/detete/" + $scope.tempLotId);
        $route.reload();
    };


    $scope.search = function () {
        tempCategory = $scope.selectedCategory.trim();
        tempSubcategory = $scope.selectedSubcategory.trim();
        tempSearchField = $scope.searchField.trim();
        $route.reload();
    };

    $scope.clearSearch = function () {
        tempCategory = "";
        tempSubcategory = "";
        tempSearchField = "";
        $route.reload();
    };

    $scope.changeBet = function (lotId, newBet) {
        $http.put('http://localhost:58135/api/lot/changeBet/' + newBet + "/" + tempUserId + "/" + lotId)
            .then(
            function (response) {
                $scope.messageField = response.data
                $route.reload();
            },
            function (response) {
                $scope.showWarningMessage = true;
                if (!response.data.Message.includes('No HTTP resource was found') &&
                    !response.data.Message.includes('The request is invalid')) $scope.warningMessage = response.data.Message;
                else $scope.warningMessage = "Correct your entered data!"
            });
    };

    $scope.showSpecification = function (Name, Specification, OwnerId, LotId) {
        $scope.name = Name;
        $scope.specification = Specification;
        if (tempUserId == OwnerId) $scope.owner = true;
        else $scope.owner = false;
        alert(tempUserId + " " + OwnerId + " " + $scope.owner);
        tempLotId = LotId;
    };

    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        $route.reload();
    };

    var finded = false;
    $scope.lookSubc = function () {
        if (finded == true) {
            name = $scope.name;
            specification = $scope.specification;
            bet = $scope.bet;
            duration = $scope.duration;
            step = $scope.step;
            $route.reload();
        }
    }
    $scope.findSubcstegories = function () {
        $http.get("http://localhost:58135/api/subcategory/get/" + $scope.selectedCategory.trim()).then(function (response) {
            $scope.subcategories = response.data
            finded = true;
        });
    }
});


app.controller('managerMenu', function ($scope, $http, $route) {
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    $scope.specification = '';
    if (tempUserLogin != "") $scope.logged = true;
    $http.get("http://localhost:58135/api/lot/GetUnconfirmedLots").then(function (response) {
        $scope.unconfirmedlots = response.data;
    });

    $http.get("http://localhost:58135/api/lot/GetEndedLots").then(function (response) {
        $scope.endedlots = response.data;
    });

    $scope.confirmLot = function (lotid) {
        $http.put('http://localhost:58135/api/lot/confirm/' + lotid).then(function (response) {
            $route.reload();
        });
    };

    $scope.deleteLot = function (Lotid) {
        $http.delete("http://localhost:58135/api/lot/detete/" + Lotid).then(function (response) {
            $route.reload();
        });
    };

    $scope.showSpecification = function (Specification) {
        $scope.specification = Specification;
    };

    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        window.location.href = "http://localhost:58352/index.html#!/lotsView";
    };
});

app.controller('adminMenu', function ($scope, $http, $route) {
    $scope.manager = isManager;
    $scope.admin = isAdmin;
    if (tempUserLogin != "") $scope.logged = true;

    $http.get("http://localhost:58135/api/subcategory").then(function (response) {
        $scope.subcategories = response.data;
    });
    $http.get("http://localhost:58135/api/category").then(function (response) {
        $scope.categories = response.data;
    });

    $scope.saveSubcategory = function () {
        var categ = $scope.selectedCategory;
        if (categ == undefined) categ = "";
        $http.post("http://localhost:58135/api/saveSubcategory/" + categ.trim() + "/" + $scope.subcategName)
            .then(function (response) {
                alert(response.data);
                $route.reload();
            });
    };

    $scope.deleteSubcategory = function (Id) {
        $http.delete("http://localhost:58135/api/deleteSubcategory/" + Id).then(function (response) {
            $route.reload();
        });
    };

    $scope.saveCategory = function () {
        $http.post("http://localhost:58135/api/saveCategory/" + $scope.Name).then(function (response) {
            alert(response.data);
            $route.reload();
        });
    };

    $scope.deleteCategory = function (Id) {
        $http.delete('http://localhost:58135/api/deleteCaregory/' + Id).then(function (response) {
            $route.reload();
        });
    };

    $scope.editSubcategory = function (CName) {
        $scope.categName = CName;
    };

    $scope.logout = function () {
        tempUserLogin = "";
        tempUserId = "";
        isManager = false; isAdmin = false;
        window.location.href = "http://localhost:58352/index.html#!/lotsView";
    };
});