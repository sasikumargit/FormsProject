app.directive('sceneWitnessDirective', function () {
    return function (scope, element, attrs) {
        if (scope.$last) {
            var mask = new RegExp('^[A-Za-z0-9 ]*$')
            $(".alphaNumeric").regexMask(mask)
        }
    };
});
