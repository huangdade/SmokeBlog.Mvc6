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
    export interface IGetArticleListRequest {
        pageIndex: number;
        pageSize: number;
        keywords: string;
        status: number;
    }
    export interface IAddArticleRequest {
        title: string;
        content: string;
        summary: string;
        userID: number;
        from?: string;
        postDate?: string;
        allowComment: boolean;
        category?: string;
        status?: number;
    }
    export interface IEditArticleRequest extends IAddArticleRequest {
        id: number;
    }
    export interface IChangeArticleStatusRequest {
        ids: number[];
        status: number;
    }
    export interface IQueryCommentRequest {
        pageIndex: number;
        pageSize: number;
        keywords: string;
        status: number;
    }
}

interface JQuery {
    selectpicker(options?: any);

    tagsinput();
}