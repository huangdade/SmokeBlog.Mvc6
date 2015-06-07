declare module BlogAdmin.Api {
    export interface IAddUserRequest {
        userName: string;
        password: string;
        email: string;
        nickname: string;
    }
    export interface IEditUserRequest {
        id: number;
        email: string;
        nickname: string;
    }
    export interface IUpdateInfoRequest {
        nickname: string;
        email: string;
    }
    export interface IChangePasswordRequest {
        oldPassword: string;
        newPassword: string;
    }
    export interface IAddCategoryRequest {
        parentID: number;
        name: string;
    }
    export interface IEditCategoryRequest {
        id: number;
        parentID: number;
        name: string;
    }
}

interface JQuery {
    selectpicker(options?: any);
}