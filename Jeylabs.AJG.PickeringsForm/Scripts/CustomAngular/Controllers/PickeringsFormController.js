
app.controller("PickeringsFormController", ['$scope', 'CrudFactory', '$http', '$filter', function ($scope, CrudFactory, $http, $filter) {
    $scope.showSuccessMessageBox = false;
    $scope.showSoundrySuccessBox = false;
    $scope.requiredHowLossDescriptionArea = false;
    $scope.yesNoOptions = ["Yes", "No"];
    $scope.insuredOptions = [];
    $scope.itemAge = ["0-3 years", "3-7 years", "7-10 years", "10+ years"];
    $scope.isInsured = false;
    $scope.isThirdPartyBlame = false;
    $scope.isReceiveNoticeThirdParty = false;
    $scope.isWitnessOnScene = false;
    $scope.isBurgularyOrTheft = false;
    $scope.isThirdPartyLiabilityClaim = false;
    $scope.isThirdPartyCorrespondence = false;
    $scope.isAdmissionofLiability = false;
    $scope.isValidated = false;
    $scope.isEmailConfirmNotValid = false;
    $scope.isDeclared = false;
    $scope.isValidatedWitness = false;
    $scope.isValidatedThirdPartyBlame = false;
    $scope.isValidatedSupportingFileUpload = false;
    $scope.isValidatedThirdPartyFileUpload = false;
    $scope.isValidatedClaim = false;
    $scope.ThirdPartyTotalFilesUploadSize = 0;
    $scope.isThirdPartyTotalFilesExceeded = false;
    $scope.SupportingFileUploadSize = 0;
    $scope.isSupportingFileUploadExceeded = false;
    $scope.claimsAdvice = {};
    $scope.ThirdPartyFileUpload = [];
    $scope.thirdPartyBlames = [];
    $scope.SceneWitness = [];
    $scope.Claim = [{ 'propertyDescription': "", 'itemAge': "", 'originalCost': '', 'replacementValue': '', 'amountClaimed': '' }];
    $scope.SupportingFileUpload = [{ 'uploadDescription': "", 'File': "", 'uploadFilename': "" }];
    $scope.TotalAmountClaimed = 0;

    $scope.getTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.Claim.length; i++) {
            var amount = $scope.Claim[i];
            total += amount.amountClaimed;
        }
        $scope.claimsAdvice.TotalAmountClaimed = total;
    }


    // CRUD Operations - Start

    $scope.onSubmit = function () {

        $("#wait").css("display", "block");
        $(".MainContaner").css("opacity", 0.5);
        $("#agree").prop('disabled', true);
        $("#SubmitClaim").prop('disabled', true);
        $("#EditClaim").prop('disabled', true);

        if ($scope.isDeclared) {
            $scope.claimsAdvice.DeclarationChecked = "Yes";
            var formData = new FormData();

            angular.forEach($scope.ThirdPartyFileUpload, function ($scope) {
                $scope.uploadFilename = $scope.File.name;
            });

            angular.forEach($scope.SupportingFileUpload, function ($scope) {
                $scope.uploadFilename = $scope.File.name;
            });

            $scope.claimsAdvice.ThirdPartyBlames = $scope.thirdPartyBlames;
            $scope.claimsAdvice.SceneWitnesses = $scope.SceneWitness;
            $scope.claimsAdvice.Claims = $scope.Claim;
            $scope.claimsAdvice.ThirdPartyFileUploads = $scope.ThirdPartyFileUpload;
            $scope.claimsAdvice.SupportingFileUploads = $scope.SupportingFileUpload;

            $http({
                method: 'POST',
                url: getDataRoot() + "Pickerings/FormPost",
                headers: { 'Content-Type': undefined },

                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("claimsAdvice", angular.toJson(data.model));
                    for (let i = 0; i < data.SupportingFileUpload.length; i++) {
                        formData.append("SupportingFileUpload[" + i + "]", data.SupportingFileUpload[i].File);
                    }

                    for (let i = 0; i < data.ThirdPartyFileUpload.length; i++) {
                        formData.append("ThirdPartyFileUpload[" + i + "]", data.ThirdPartyFileUpload[i].File);
                    }

                    return formData;
                },
                data: {
                    model: $scope.claimsAdvice,
                    SupportingFileUpload: $scope.SupportingFileUpload,
                    ThirdPartyFileUpload: $scope.ThirdPartyFileUpload
                }
            }).
            then(function (data, status, headers, config) {

                window.location = getDataRoot() + "Confirmation/Acknowledgement?referenceNumber=" + data['data'][0] + "&submitedDate=" + data['data'][1];

            }, function () {
                window.location = getDataRoot() + "Error/Error";
            });
        }
        else {
            $scope.isValidated = true;
        }

    }

    CrudFactory.get(getDataRoot() + "Pickerings/AllInsuredDetails", {

    }).then(function (res) {
        $scope.insuredOptions = res.data;
        $scope.insuredOptions.push({ 'id': "Other", 'BusinessName': "Other", 'ABN': "", 'BusinessAddress': "" });
    }, function (err) {
        console.log(err);
    });

    // CRUD Operations - End 

    // Drop down Change Events  - Start

    $scope.onInsuredChange = function (insured) {

        if (insured === "Other") {
            $scope.isInsured = true;
            $scope.claimsAdvice.insuredBusinessName = "";
            $scope.claimsAdvice.insuredABN = "";
            $scope.claimsAdvice.InsuredBusinessAddress = "";
        }
        else {
            $scope.isInsured = false;
            $.each($scope.insuredOptions, function (k, v) {
                if (v.id == insured) {
                    $scope.claimsAdvice.insuredBusinessName = v.BusinessName;
                    $scope.claimsAdvice.insuredABN = v.ABN;
                    $scope.claimsAdvice.InsuredBusinessAddress = v.BusinessAddress;
                }
            });

        }
    }

    $scope.onThirdPartyBlameChange = function (status) {

        if (status === "Yes") {

            $scope.isThirdPartyBlame = true;
            $scope.addNew();
        }
        else {
            $scope.isThirdPartyBlame = false;
            $scope.thirdPartyBlames = [];
        }
    }

    $scope.onReceiveNoticeThirdPartyChange = function (status) {

        if (status === "Yes") {
            $scope.isReceiveNoticeThirdParty = true;
        }
        else {
            $scope.isReceiveNoticeThirdParty = false;
        }
    }

    $scope.onWitnessonSceneChanged = function (status) {
        if (status == "Yes") {
            $scope.isWitnessOnScene = true;
            $scope.addNewWitness(true);
        }
        else {
            $scope.isWitnessOnScene = false;
            $scope.SceneWitness = [];
        }
    }
    $scope.onBurgularyOrTheftChanged = function (status) {
        if (status == "Yes") {
            $scope.isBurgularyOrTheft = true;
        }
        else {
            $scope.claimsAdvice.PoliceStation = "";
            $scope.claimsAdvice.PoliceOfficerName = "";
            $scope.claimsAdvice.PoliceReportNumber = "";
            $scope.isBurgularyOrTheft = false;
        }
    }

    $scope.onThirdPartyLiabilityClaimChanged = function (status) {
        if (status == "Yes") {
            $scope.isThirdPartyLiabilityClaim = true;
        }
        else {
            $scope.isThirdPartyLiabilityClaim = false;
        }
    }

    $scope.onThirdPartyCorrespondenceChanged = function (status) {
        if (status == "Yes") {
            $scope.isThirdPartyCorrespondence = true;
            $scope.addNewThirdPartyFileUpload();
        }
        else {
            $scope.isThirdPartyCorrespondence = false;
            $scope.ThirdPartyFileUpload = [];
            $scope.isThirdPartyTotalFilesExceeded = false;
        }
    }

    $scope.onAdmissionofLiabilityChanged = function (status) {
        if (status == "Yes") {
            $scope.isAdmissionofLiability = true;
        }
        else {
            $scope.isAdmissionofLiability = false;
        }
    }

    // Drop down Change Events  - End 

    // Third Party Blames Grid for loss or damage- Start 

    $scope.addNew = function () {

        if ($scope.thirdPartyBlames.length == 0 || $scope.thirdPartyBlames[$scope.thirdPartyBlames.length - 1].name != '' && $scope.thirdPartyBlames[$scope.thirdPartyBlames.length - 1].registration != '' && $scope.thirdPartyBlames[$scope.thirdPartyBlames.length - 1].address != '' && $scope.thirdPartyBlames[$scope.thirdPartyBlames.length - 1].phone != '') {
            $scope.thirdPartyBlames.push({
                'name': "",
                'registration': "",
                'address': "",
                'phone': ""
            });

            $('.thirdPartyBlameCheckbox').attr('checked', false);
            $scope.isValidatedThirdPartyBlame = false;
        }
        else {
            $scope.isValidatedThirdPartyBlame = true;
        }
    };

    $scope.remove = function () {
        $scope.isValidatedThirdPartyBlame = false;
        var newDataList = [];
        $scope.selectedAll = false;
        $('.thirdPartyBlameCheckbox').attr('checked', false);
        angular.forEach($scope.thirdPartyBlames, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.thirdPartyBlames = newDataList;
        if (newDataList.length == 0) {
            $scope.addNew();
        }
    };

    $scope.checkAll = function () {
        if (!$scope.selectedAll) {
            $scope.selectedAll = true;
        } else {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.thirdPartyBlames, function (thirdPartyBlame) {
            thirdPartyBlame.selected = $scope.selectedAll;
        });
    };

    // Third Party Blames Grid for loss or damage- Start


    // Scene Witness  or damage- Start

    $scope.addNewWitness = function (witness) {
        if ($scope.SceneWitness.length == 0 || $scope.SceneWitness[$scope.SceneWitness.length - 1].name != '' && $scope.SceneWitness[$scope.SceneWitness.length - 1].registration != '' && $scope.SceneWitness[$scope.SceneWitness.length - 1].address != '' && $scope.SceneWitness[$scope.SceneWitness.length - 1].phone != '') {
            $scope.SceneWitness.push({
                'name': "",
                'registration': "",
                'address': "",
                'phone': ""
            });
            $('.witnessCheckbox').attr('checked', false);
            $scope.isValidatedWitness = false;
        }
        else {
            $scope.isValidatedWitness = true;
        }
    };

    $scope.removeWitness = function () {
        $scope.isValidatedWitness = false;
        var newDataList = [];
        $scope.selectedAllwitness = false;
        $('.witnessCheckbox').attr('checked', false);
        angular.forEach($scope.SceneWitness, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.SceneWitness = newDataList;
        if (newDataList.length == 0) {
            $scope.addNewWitness();
        }
    };

    $scope.checkAllWitness = function () {
        if (!$scope.selectedAllwitness) {
            $scope.selectedAllwitness = true;
        } else {
            $scope.selectedAllwitness = false;
        }
        angular.forEach($scope.SceneWitness, function (witness) {
            witness.selected = $scope.selectedAllwitness;
        });
    };

    // Scene Witness Grid - Start

    // Claim Grid - Start 


    $scope.addNewClaim = function (claim) {
        if ($scope.Claim[$scope.Claim.length - 1].propertyDescription != '' && $scope.Claim[$scope.Claim.length - 1].itemAge != "" && $scope.Claim[$scope.Claim.length - 1].originalCost != 0 && $scope.Claim[$scope.Claim.length - 1].replacementValue != 0 && $scope.Claim[$scope.Claim.length - 1].amountClaimed != 0) {
            $scope.Claim.push({
                'propertyDescription': "",
                'itemAge': "",
                'originalCost': '',
                'replacementValue': '',
                'amountClaimed': ''
            });
            $('.ClaimCheckbox').attr('checked', false);
            $scope.isValidatedClaim = false;
        }
        else {
            $scope.isValidatedClaim = true;
        }
    };

    $scope.removeClaim = function () {
        $scope.isValidatedClaim = false;
        var newDataList = [];
        $scope.selectedAllClaim = false;
        $('.ClaimCheckbox').attr('checked', false);
        angular.forEach($scope.Claim, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.Claim = newDataList;
        $scope.getTotal();
        if (newDataList.length == 0) {
            $scope.addNewClaim();
        }
    };

    $scope.checkAllClaim = function () {
        if ($scope.selectedAllClaim) {
            $scope.selectedAllClaim = true;
        } else {
            $scope.selectedAllClaim = false;
        }
        angular.forEach($scope.Claim, function (claim) {
            claim.selected = $scope.selectedAllClaim;
        });
    };
    // Claim Grid - End        

    $scope.addNewThirdPartyFileUpload = function (thirdPartyFileUpload) {

        if ($scope.ThirdPartyFileUpload.length == 0 || ($scope.ThirdPartyFileUpload[$scope.ThirdPartyFileUpload.length - 1].uploadDescription != '' && $scope.ThirdPartyFileUpload[$scope.ThirdPartyFileUpload.length - 1].File != '')) {
            $scope.ThirdPartyFileUpload.push({
                'uploadDescription': "",
                'File': "",
                'uploadFilename': ""
            });
            $('.ThirdPartyFileUploadCheckbox').attr('checked', false);
            $scope.isValidatedThirdPartyFileUpload = false;
        }
        else {
            $scope.isValidatedThirdPartyFileUpload = true;
        }
    };

    $scope.ThirdPartyFileUploadInput = function (input) {

        $scope.isThirdPartyTotalFilesExceeded = false;
        $("#ThirdpartyErrorlabel").hide();
        if (input.files.length > 0) {
            for (var i = 0; i <= input.files.length - 1; i++) {
                var fsize = input.files.item(i).size;
                var inMB = (fsize / 1024) / 1024;
                $scope.ThirdPartyTotalFilesUploadSize = $scope.ThirdPartyTotalFilesUploadSize + (inMB * 1);
            }
            if ($scope.ThirdPartyTotalFilesUploadSize <= 22) {
                $scope.ThirdPartyFileUpload[$(input).data('inputKey')].File = input.files[0];
            }
            else {
                $scope.isThirdPartyTotalFilesExceeded = true;
                $("#ThirdpartyErrorlabel").show();
                //var newDataList = [];
                $scope.ThirdPartyTotalFilesUploadSize = 0;
                for (var i = 0; i <= $scope.ThirdPartyFileUpload.length - 1; i++) {
                    var file = $scope.ThirdPartyFileUpload[i].File;
                    if (file != "") {
                        var fsize = file.size;
                        var inMB = (fsize / 1024) / 1024;
                        $scope.ThirdPartyTotalFilesUploadSize = $scope.ThirdPartyTotalFilesUploadSize + (inMB * 1);
                        //newDataList.push($scope.ThirdPartyFileUpload[i]);
                    }
                }
                //$scope.ThirdPartyFileUpload = newDataList;
            }
        } else {
            $scope.ThirdPartyFileUpload[$(input).data('inputKey')].File = null;

        }
    }

    $scope.removeThirdPartyFileUpload = function () {
        $scope.isValidatedThirdPartyFileUpload = false;
        var newDataList = [];
        $scope.selectedAllThirdPartyFileUpload = false;
        $('.ThirdPartyFileUploadCheckbox').attr('checked', false);
        angular.forEach($scope.ThirdPartyFileUpload, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.ThirdPartyFileUpload = newDataList;
        $scope.ThirdPartyTotalFilesUploadSize = 0;
        for (var i = 0; i <= $scope.ThirdPartyFileUpload.length - 1; i++) {
            var file = $scope.ThirdPartyFileUpload[i].File;
            if (file != "") {
                var fsize = file.size;
                var inMB = (fsize / 1024) / 1024;
                $scope.ThirdPartyTotalFilesUploadSize = $scope.ThirdPartyTotalFilesUploadSize + (inMB * 1);
            }
        }

        if ($scope.ThirdPartyTotalFilesUploadSize <= 22) {
            $scope.isThirdPartyTotalFilesExceeded = false;
            $("#ThirdpartyErrorlabel").hide();
        }
        else {
            $scope.isThirdPartyTotalFilesExceeded = true;
            $("#ThirdpartyErrorlabel").show();
        }
        if (newDataList.length == 0) {
            $scope.addNewThirdPartyFileUpload();
        }
    };

    $scope.checkAllThirdPartyFileUpload = function () {
        if (!$scope.selectedAllThirdPartyFileUpload) {
            $scope.selectedAllThirdPartyFileUpload = true;
        } else {
            $scope.selectedAllThirdPartyFileUpload = false;
        }
        angular.forEach($scope.ThirdPartyFileUpload, function (thirdPartyFileUpload) {
            thirdPartyFileUpload.selected = $scope.selectedAllThirdPartyFileUpload;
        });
    };
    // Third Party File Upload Grid - End 


    //Support File Upload Grid - Start  

    $scope.addNewSupportingFileUpload = function (supportFileUpload) {
        if ($scope.SupportingFileUpload.length == 0 || ($scope.SupportingFileUpload[$scope.SupportingFileUpload.length - 1].uploadDescription != '' && $scope.SupportingFileUpload[$scope.SupportingFileUpload.length - 1].File != '')) {
            $scope.SupportingFileUpload.push({
                'uploadDescription': "",
                'File': "",
                'uploadFilename': ""
            });
            $('.SupportingFileUploadCheckbox').attr('checked', false);
            $scope.isValidatedSupportingFileUpload = false;
        }
        else {
            $scope.isValidatedSupportingFileUpload = true;
        }
    };

    $scope.SupportingFileUploadInput = function (input) {
        if (input.files.length > 0) {

            $scope.isSupportingFileUploadExceeded = false;
            $("#SupportingFileErrorLabel").hide();
            if (input.files.length > 0) {
                for (var i = 0; i <= input.files.length - 1; i++) {
                    var fsize = input.files.item(i).size;
                    var inMB = (fsize / 1024) / 1024;
                    $scope.SupportingFileUploadSize = $scope.SupportingFileUploadSize + (inMB * 1);
                }
                if ($scope.SupportingFileUploadSize <= 22) {
                    $scope.SupportingFileUpload[$(input).data('inputKey')].File = input.files[0];
                }
                else {
                    $scope.isSupportingFileUploadExceeded = true;
                    $("#SupportingFileErrorLabel").show();

                    //var newDataList = [];
                    $scope.SupportingFileUploadSize = 0;
                    for (var i = 0; i <= $scope.SupportingFileUpload.length - 1; i++) {
                        var file = $scope.SupportingFileUpload[i].File;
                        if (file != "") {
                            var fsize = file.size;
                            var inMB = (fsize / 1024) / 1024;
                            $scope.SupportingFileUploadSize = $scope.SupportingFileUploadSize + (inMB * 1);
                            //newDataList.push($scope.ThirdPartyFileUpload[i]);
                        }
                    }
                    //$scope.ThirdPartyFileUpload = newDataList;
                }
            } else {
                $scope.SupportingFileUpload[$(input).data('inputKey')].File = null;
            }
        }
    }

    $scope.removeSupportingFileUpload = function () {
        $scope.isValidatedSupportingFileUpload = false;
        var newDataList = [];
        $scope.selectedAllSupportingFileUpload = false;
        $('.SupportingFileUploadCheckbox').attr('checked', false);
        angular.forEach($scope.SupportingFileUpload, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });




        $scope.SupportingFileUpload = newDataList;
        $scope.SupportingFileUploadSize = 0;
        for (var i = 0; i <= $scope.SupportingFileUpload.length - 1; i++) {
            var file = $scope.SupportingFileUpload[i].File;
            if (file != "") {
                var fsize = file.size;
                var inMB = (fsize / 1024) / 1024;
                $scope.SupportingFileUploadSize = $scope.SupportingFileUploadSize + (inMB * 1);
            }
        }

        if ($scope.SupportingFileUploadSize <= 22) {
            $scope.isSupportingFileUploadExceeded = false;
            $("#SupportingFileErrorLabel").hide();

        }
        else {
            $scope.isSupportingFileUploadExceeded = true;
            $("#SupportingFileErrorLabel").show();

        }
        if (newDataList.length == 0) {
            $scope.addNewSupportingFileUpload();
        }
    };

    $scope.checkAllSupportingFileUpload = function () {
        if ($scope.selectedAllSupportingFileUpload) {
            $scope.selectedAllSupportingFileUpload = true;
        } else {
            $scope.selectedAllSupportingFileUpload = false;
        }
        angular.forEach($scope.SupportingFileUpload, function (supportingFileUpload) {
            supportingFileUpload.selected = $scope.selectedAllSupportingFileUpload;
        });
    };

    $scope.GoToPreview = function () {
        $scope.isValidated = !$scope.ValidateForm();
        if (!$scope.isValidated) {
            $('form input').attr('disabled', true);
            $('form select').attr('disabled', true);
            $('form textarea').attr('disabled', true);
            $("#declaration").show();
            $("#PreviewButtonsHolder").show();
            $("#nextButtonHolder").hide();
            $("#agree").attr('disabled', false);
        }
    }
    $scope.ValidateEmail = function (data1, data2) {
        if (data1 != data2) {
            $scope.isEmailConfirmNotValid = true;
        }
        else {
            $scope.isEmailConfirmNotValid = false;

        }
    }

    $scope.ValidateForm = function () {
        var isFormValid = false;
        $scope.claimsAdvice.EventAddress = $('#EventAddress').val();
        $scope.claimsAdvice.DateofEvent = $('#DateofEvent').val();
        $scope.claimsAdvice.DateofEvent = $('#DateofEvent').val();
        $scope.claimsAdvice.TimeofDay = $('#TimeofDay').val();
        $scope.claimsAdvice.IPAddress = $("#IPAddress").val();
        if ($scope.isThirdPartyTotalFilesExceeded || $scope.isSupportingFileUploadExceeded) {
            isFormValid = false;
        }
        else {
            if ($scope.claimsAdvice.insured != undefined && $scope.claimsAdvice.ContactName != undefined && $scope.claimsAdvice.ContactEmail != undefined && $scope.claimsAdvice.DateofEvent != undefined && $scope.claimsAdvice.TimeofDay != undefined
               && $scope.claimsAdvice.EventAddress != undefined && $scope.claimsAdvice.HowLossDescription != undefined && $scope.claimsAdvice.ThirdPartyBlame != undefined && $scope.claimsAdvice.ReceiveNoticeThirdParty != undefined
               && $scope.claimsAdvice.BurglaryorTheft != undefined && $scope.claimsAdvice.ThirdPartyLiabilityClaim != undefined && $scope.Claim != undefined && $scope.claimsAdvice.WitnessesatScene != undefined && $scope.isEmailConfirmNotValid == false) {
                isFormValid = true;

                if ($scope.claimsAdvice.insured == "Other") {
                    if ($scope.claimsAdvice.insuredBusinessName == undefined) {
                        $("#insuredBusinessName").focusInvalid();
                        isFormValid = false;
                    }
                    if ($scope.claimsAdvice.insuredABN == undefined) {
                        $("#insuredABN").focusInvalid();
                        isFormValid = false;
                    }
                    if ($scope.claimsAdvice.InsuredBusinessAddress == undefined) {
                        $("#InsuredBusinessAddress").focusInvalid();
                        isFormValid = false;
                    }
                }
                if ($scope.claimsAdvice.ThirdPartyBlame == "Yes") {
                    if ($scope.thirdPartyBlames == undefined) {
                        $("#ThirdPartyBlame").focusInvalid();
                        isFormValid = false;
                    }
                }
                if ($scope.claimsAdvice.ReceiveNoticeThirdParty == "Yes") {
                    if ($scope.claimsAdvice.ReceiveNoticeThirdPartyDetails == '') {
                        $("#ReceiveNoticeThirdPartyDetails").focusInvalid();
                        isFormValid = false;
                    }
                }
                if ($scope.claimsAdvice.DateofEvent == "" || $scope.claimsAdvice.DateofEvent == undefined) {
                   isFormValid = false;    
                }
                if ($scope.claimsAdvice.TimeofDay == "" || $scope.claimsAdvice.TimeofDay == undefined) {
                    isFormValid = false;
                }
                if ($scope.claimsAdvice.EventAddress == "" || $scope.claimsAdvice.EventAddress == undefined) {
                    isFormValid = false;
                }
                if ($scope.claimsAdvice.WitnessesatScene == "Yes") {
                    if ($scope.SceneWitness == undefined) {
                        isFormValid = false;
                    }
                }
                if ($scope.claimsAdvice.BurglaryorTheft == "Yes") {
                    if ($scope.claimsAdvice.PoliceStation == undefined) {
                        $("#PoliceStation").focusInvalid();
                        isFormValid = false;
                    }
                    if ($scope.claimsAdvice.PoliceOfficerName == undefined) {
                        $("#PoliceOfficerName").focusInvalid();
                        isFormValid = false;
                    }
                    if ($scope.claimsAdvice.PoliceReportNumber == undefined) {
                        $("#PoliceReportNumber").focusInvalid();
                        isFormValid = false;
                    }

                }
                if ($scope.claimsAdvice.ThirdPartyLiabilityClaim == "Yes") {
                    if ($scope.claimsAdvice.ThirdPartyName == undefined || $scope.claimsAdvice.ThirdPartyPermanentAddress == undefined || $scope.claimsAdvice.InjuriesDamageDescription == undefined) {
                        isFormValid = false;
                    }

                    if ($scope.claimsAdvice.ThirdPartyCorrespondence == undefined || $scope.claimsAdvice.AdmissionofLiability == undefined) {
                        isFormValid = false;
                    }
                    if ($scope.claimsAdvice.ThirdPartyCorrespondence == "Yes") {

                        if ($scope.ThirdPartyFileUpload.length == 0 || ($scope.ThirdPartyFileUpload[$scope.ThirdPartyFileUpload.length - 1].uploadDescription != '' && $scope.ThirdPartyFileUpload[$scope.ThirdPartyFileUpload.length - 1].File != '')) {
                            $scope.isValidatedThirdPartyFileUpload = false;
                        }
                        else {
                            $scope.isValidatedThirdPartyFileUpload = true;
                        }
                    }

                    if ($scope.claimsAdvice.ThirdPartyCorrespondence == "Yes") {
                        if ($scope.ThirdPartyFileUpload == undefined) {
                            isFormValid = false;
                        }
                    }
                    if ($scope.claimsAdvice.AdmissionofLiability == "Yes") {
                        if ($scope.claimsAdvice.AdmissionofLiabilityDetails == undefined) {
                            isFormValid = false;
                        }
                    }

                }
            }
            else {
                isFormValid = false;
            }
        }
        for (var i = 0; i < $scope.Claim.length; i++) {
            if (($scope.Claim[i].propertyDescription != '' && $scope.Claim[i].propertyDescription != undefined) && ($scope.Claim[i].itemAge != "" && $scope.Claim[i].itemAge != undefined) && ($scope.Claim[i].originalCost != 0 && $scope.Claim[i].originalCost != undefined) && ($scope.Claim[i].replacementValue != 0 && $scope.Claim[i].replacementValue != undefined) && ($scope.Claim[i].amountClaimed != 0 && $scope.Claim[i].amountClaimed != undefined)) {
                $scope.isValidatedClaim = false;
            }
            else {
                $scope.isValidatedClaim = true;
                isFormValid = false;
                break;
            }
        }
        if ($scope.claimsAdvice.ThirdPartyBlame == "Yes") {
            for (var i = 0; i < $scope.thirdPartyBlames.length; i++) {
                if (($scope.thirdPartyBlames[i].name != '' && $scope.thirdPartyBlames[i].name != undefined) && ($scope.thirdPartyBlames[i].registration != '' && $scope.thirdPartyBlames[i].registration != undefined) && ($scope.thirdPartyBlames[i].address != '' && $scope.thirdPartyBlames[i].address != undefined) && ($scope.thirdPartyBlames[i].phone != '' && $scope.thirdPartyBlames[i].phone != undefined)) {
                    $scope.isValidatedThirdPartyBlame = false;
                }
                else {
                    $scope.isValidatedThirdPartyBlame = true;
                    isFormValid = false;
                    break;
                }
            }
        }

        if ($scope.claimsAdvice.WitnessesatScene == "Yes")
            for (var i = 0; i < $scope.SceneWitness.length; i++) {
                if (($scope.SceneWitness[i].name != '' && $scope.SceneWitness[i].name != undefined) && ($scope.SceneWitness[i].registration != '' && $scope.SceneWitness[i].registration != undefined) && ($scope.SceneWitness[i].address != '' && $scope.SceneWitness[i].address != undefined) && ($scope.SceneWitness[i].phone != '' && $scope.SceneWitness[i].phone != undefined)) {
                    $scope.isValidatedClaim = false;
                }
                else {
                    $scope.isValidatedClaim = true;
                    isFormValid = false;
                    break;
                }
            }
        if ($scope.claimsAdvice.ReceiveNoticeThirdParty == "Yes") {
            if ($scope.claimsAdvice.ReceiveNoticeThirdPartyDetails.length == 0) {
                isFormValid = false;
            }
        }
        if ($scope.claimsAdvice.BurglaryorTheft == "Yes") {
            if ($scope.claimsAdvice.PoliceStation.length == 0 || $scope.claimsAdvice.PoliceOfficerName.length == 0 || $scope.claimsAdvice.PoliceReportNumber.length == 0) {
                isFormValid = false;
            }
        }
        if ($scope.claimsAdvice.ThirdPartyCorrespondence == "Yes") {
            for (var i = 0; i < $scope.ThirdPartyFileUpload.length; i++) {
                if ($scope.ThirdPartyFileUpload.length == 0 || (($scope.ThirdPartyFileUpload[i].uploadDescription != '' && $scope.ThirdPartyFileUpload[i].uploadDescription != undefined) && $scope.ThirdPartyFileUpload[i].File != '')) {
                    $scope.isValidatedThirdPartyFileUpload = false;
                }
                else {
                    $scope.isValidatedThirdPartyFileUpload = true;
                    isFormValid = false;
                    break;
                }
            }
        }
        if ($scope.claimsAdvice.ConfirmContactEmail != undefined) {
            if (!($scope.claimsAdvice.ConfirmContactEmail == $scope.claimsAdvice.ContactEmail)) {
                $scope.isEmailConfirmNotValid = true;
                document.getElementById("ConfirmContactEmail").setCustomValidity("Confirm Email Doesn't Match");
                isFormValid = false;
            }
            else
            {
                $scope.isEmailConfirmNotValid = false;
                document.getElementById("ConfirmContactEmail").setCustomValidity("");
            }
        }
        else {
            $scope.isEmailConfirmNotValid = false;
            isFormValid = false;
        }
        if ($scope.claimsAdvice.HowLossDescription == undefined) {
            $scope.requiredHowLossDescriptionArea = true;
            isFormValid = false;
        }
        else
        {
            $scope.requiredHowLossDescriptionArea = false;
        }
        return isFormValid;
    }
    $scope.GoToEdit = function () {
        $scope.isValidated = !$scope.ValidateForm();
        if (!$scope.isValidated) {
            $('form input').attr('disabled', false);
            $('form select').attr('disabled', false);
            $('form textarea').attr('disabled', false);
            $("#declaration").hide();
            $("#PreviewButtonsHolder").hide();
            $("#nextButtonHolder").show();
        }
    }

    $scope.OnDeclaredChange = function (status) {
        $scope.isDeclared = status;
    }

    //Support File Upload Grid - End 
}])



function getDataRoot() {
    var root = null;

    if (document.body) {
        root = document.body.getAttribute('data-root');
    }

    if (!root) {
        return '/';
    }

    return root;
}

var placeSearch, autocomplete;
function initAutocomplete() {
    var countryRestrict = { 'country': 'au' };
    // Create the autocomplete object, restricting the search to geographical
    // location types.
    autocomplete = new google.maps.places.Autocomplete(
        (document.getElementById('EventAddress')),
        {
            types: ['geocode'],
            componentRestrictions: countryRestrict
        });
    // When the user selects an address from the dropdown, populate the address
    // fields in the form.
    //autocomplete.addListener('place_changed', fillInAddress);
}
//Test: Print the IP addresses into the console


