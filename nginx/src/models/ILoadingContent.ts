export interface ILoadingContent<T> {
    loading?: boolean;
    errors?: any;
    content: T;
}