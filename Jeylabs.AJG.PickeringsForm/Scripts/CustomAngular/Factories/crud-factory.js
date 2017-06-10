app.factory("CrudFactory", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //var base_url = "http://localhost:15518/Home";
    var base_url = "";
    return {
        
        get: function (url) {
            var deferred = $q.defer();
            $http({
                method: 'GET',
                url: base_url + url
                
            }).then(function (res) {

                deferred.resolve(res);

            }).then(function (err) {

                deferred.reject(err);
            });

            return deferred.promise;
        },

        post: function (url, user_data) {
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: base_url + url,
                data: user_data
            }).success(function (res) {

                deferred.resolve(res);

            }).error(function (err) {

                deferred.reject(err);
            });

            return deferred.promise;
        },
             

        put: function (url, user_data) {
            var deferred = $q.defer();
            $http({
                method: 'PUT',
                url: base_url + url,
                data: user_data
            }).success(function (res) {

                deferred.resolve(res);

            }).error(function (err) {

                deferred.reject(err);
            });

            return deferred.promise;
        },

        delete: function (url, user_data) {
            var deferred = $q.defer();
            $http({
                method: 'DELETE',
                url: base_url + url,
                data: user_data
            }).success(function (res) {

                deferred.resolve(res);

            }).error(function (err) {

                deferred.reject(err);
            });

            return deferred.promise;
        },

    }
}
]);