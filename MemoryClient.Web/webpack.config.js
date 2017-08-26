var BundleAnalyzerPlugin = require("webpack-bundle-analyzer").BundleAnalyzerPlugin;
var webpack = require("webpack");
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var MergeJsonWebpackPlugin = require("merge-jsons-webpack-plugin");
var ReactIntlPlugin = require("react-intl-webpack-plugin");

const path = require("path");
const DEBUG = true;
const supportedLangs = ["en-US", "ja-JP"];

module.exports = {
    entry: {
        main: ["./Scripts/MainPage.tsx", "./Content/main.scss"],
        admin: ["./Scripts/AdminPage.tsx", "./Content/admin.scss"],
        vendor: "./Content/vendor.scss" 
    },
    output: {
        publicPath: "/",
        path: path.join(__dirname, "/wwwroot/"),
        filename: "js/[name].build.js"
    },
    cache: DEBUG,

    devtool: "source-map",
    /*
   * resolve lets Webpack now in advance what file extensions you plan on
   * "require"ing into the web application, and allows you to drop them
   * in your code.
   */
    resolve: {
        extensions: [".ts", ".tsx", ".js", ".json"]
    },

    module: {
        /*
         * Each loader needs an associated Regex test that goes through each
         * of the files you've included (or in this case, all files but the
         * ones in the excluded directories) and finds all files that pass
         * the test. Then it will apply the loader to that file. I haven't
         * installed ts-loader yet, but will do that shortly.
         */
        rules: [
            {
                test: /\.tsx?$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: "babel-loader",
                        options: {
                            cacheDirectory: false,
                            metadataSubscribers: [ReactIntlPlugin.metadataContextFunctionName],
                            plugins: ["transform-runtime", ["react-intl", {
                                messagesDir: "./i18n"
                            }]],
                            presets: ["react", "es6", "stage-1"]
                        }
                    },
                    { loader: "awesome-typescript-loader" }
                ]
            },
            {
                test: /\.css$/,
                exclude: /node_modules/,
                use: ExtractTextPlugin.extract({
                    loader: "css-loader",
                    options: {
                        importLoaders: 1
                    }
                })
            },
            {
                test: /\.scss$/,
                exclude: /node_modules/,
                use: ExtractTextPlugin.extract(["css-loader", "sass-loader"])
            },
            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                exclude: /node_modules/,
                use: {
                    loader: "url-loader",
                    options: {
                        limit: 10000,
                        mimetype: "application/font-woff",
                        name: "fonts/[name].[ext]"
                    }
                }
            },
            {
                test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                exclude: /node_modules/,
                use: {
                    loader: "file-loader",
                    options: {
                        name: "js/fonts/[name].[ext]"
                    }
                }
            }
        ]
    },

    plugins: [
        new ReactIntlPlugin(),
        new BundleAnalyzerPlugin({
            analyzerMode: "static",
            openAnalyzer: false
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: "node-static",
            filename: "js/node.build.js",
            minChunks(module, count) {
                const context = module.context;
                return context && context.indexOf("node_modules") >= 0;
            }
        }),
        new webpack.ProvidePlugin({
            jQuery: "jquery",
            $: "jquery",
            jquery: "jquery",
            "window.jQuery": "jquery",
            Tether: "tether",
            "window.Tether": "tether",
            Alert: "exports-loader?Alert!bootstrap/js/dist/alert",
            Button: "exports-loader?Button!bootstrap/js/dist/button",
            Carousel: "exports-loader?Carousel!bootstrap/js/dist/carousel",
            Collapse: "exports-loader?Collapse!bootstrap/js/dist/collapse",
            Dropdown: "exports-loader?Dropdown!bootstrap/js/dist/dropdown",
            Modal: "exports-loader?Modal!bootstrap/js/dist/modal",
            Popover: "exports-loader?Popover!bootstrap/js/dist/popover",
            Scrollspy: "exports-loader?Scrollspy!bootstrap/js/dist/scrollspy",
            Tab: "exports-loader?Tab!bootstrap/js/dist/tab",
            Tooltip: "exports-loader?Tooltip!bootstrap/js/dist/tooltip",
            Util: "exports-loader?Util!bootstrap/js/dist/util"
        }),
        new ExtractTextPlugin({
            filename: "css/[name].build.css",
            allChunks: true
        }),
        new MergeJsonWebpackPlugin({
            encoding: "utf-8",
            output: {
                groupBy: supportedLangs.map(lang => {
                    return {
                        pattern: `./Scripts/lang/**/${lang}.json`,
                        fileName: `lang/${lang}.json`
                    };
                })
            }
        })
    ]
};