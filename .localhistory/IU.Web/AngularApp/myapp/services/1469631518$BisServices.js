﻿//'use strict';

IUApp.factory('BisServices', ['$http', '$q', function ($http, $q) {

    return {
        acceptRegister: function (acceptRegister) {
            var def = $q.defer();
            $http.post('api/StuAttendance/AcceptRegister', acceptRegister)
                .success(function (_acceptRegister) {
                    def.resolve(_acceptRegister);
                })
                .error(function () {
                    def.reject("Failed to submit feedback");
                });
            return def.promise;
        },
        getRegisterData: function () {
            var def = $q.defer();
            $http.get("api/StuAttendance/GetRegisterData")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get Register data");
                });
            return def.promise;
        }
    };
}]);